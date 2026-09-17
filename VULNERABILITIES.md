# AbpGoat — Intentional Vulnerability Catalogue (Ground Truth)

> **WARNING**
> AbpGoat is a **deliberately insecure** ABP application. It exists only as a target
> for security scanners and penetration-testing tools. **Never deploy it to a public
> or shared environment.** Run it on localhost only.
>
> The flaws below were introduced **on purpose, in the sample application code**. They
> are **not** flaws in the ABP Framework itself. Every finding lives under an
> `AbpGoat.*.Vulnerable` namespace so it stays separated from the framework's own code.

This file is the **ground truth** used to score security tools. After a scan, compare
the tool's findings against this table: how many were found, how many false positives
were raised, and which layer (static, DAST, LLM agent) caught each one.

## Scoring table

| ID | Class | CWE | Location | ABP/.NET-specific | Static SAST | DAST (ZAP) | LLM (Strix) |
|----|-------|-----|----------|:---:|:---:|:---:|:---:|
| VL-001 | SQL injection via raw SQL search | CWE-89 | `ReportAppService.SearchAsync` | – | expected | expected | expected |
| VL-002 | Stored XSS (unescaped HTML render) | CWE-79 | `Pages/Vulnerable/Feedback` | – | maybe | expected | expected |
| VL-003 | IDOR (missing ownership check) | CWE-639 | `DocumentAppService.GetAsync` | – | miss | miss | expected |
| VL-004 | Path traversal on file download | CWE-22 | `DocumentAppService.DownloadAsync` | – | maybe | maybe | expected |
| VL-005 | SSRF (fetch user-supplied URL) | CWE-918 | `WebhookAppService.TestAsync` | – | miss | maybe | expected |
| VL-006 | Hardcoded secrets + weak crypto | CWE-798 / CWE-327 | `appsettings.json`, `LegacyHasher` | – | expected | miss | maybe |
| VL-007 | Missing `[Authorize]` on AppService method | CWE-862 | `AdminToolsAppService.ResetDocumentsAsync` | **ABP** | miss | miss | maybe |
| VL-008 | Tenant isolation bypass via `IDataFilter.Disable` | CWE-639 | `ReportAppService.GetCrossTenantAsync` | **ABP** | miss | miss | maybe |
| VL-009 | Permission declared but never checked | CWE-862 | `DocumentAppService.DeleteAsync` | **ABP** | miss | miss | maybe |
| VL-010 | Host-only endpoint reachable by tenant user | CWE-266 | `AdminToolsAppService.ListAllTenantsAsync` | **ABP** | miss | miss | maybe |
| VL-011 | OpenIddict client granted excessive scope | CWE-250 | DbMigrator OpenIddict seed | **ABP** | miss | miss | miss |
| VL-012 | Mass assignment (role list bound from input) | CWE-915 | `UserProfileAppService.UpdateAsync` | partial | maybe | miss | expected |
| VL-013 | Culture-sensitive `ToUpper()` auth bypass | CWE-178 | `AdminToolsAppService.IsPrivilegedAsync` | miss | miss | maybe |

`expected` = a competent tool of that layer should catch it. `maybe` = depends on
capability. `miss` = that layer structurally cannot see it. The `miss` column for the
ABP-specific rows is the whole reason the Phase 3 ABP-aware review skill exists.

---

## The exploit chain (VL-004 → VL-006)

The path-traversal download (VL-004) and the hardcoded secrets (VL-006) are wired to
form a **single chain**, so a tool that only reports them separately scores lower than
one that connects them:

1. `DocumentAppService.DownloadAsync` joins user input onto the file-system BLOB base
   path (`./BlobStoring`) without normalisation, so `../../appsettings.json` escapes
   the blob root.
2. `appsettings.json` contains real-looking secrets: the database password, the
   `StringEncryption:DefaultPassPhrase`, and `AuthServer:CertificatePassPhrase`.
3. Reading step 1 leaks the secret from step 2.

Score a scan on **"did it connect VL-004 and VL-006 into one attack path?"** as a
separate line. Static SAST and ZAP report the two independently; an LLM agent is the
layer that can reason from one to the other.

---

## Vulnerability details

### VL-001 — SQL injection via raw SQL search
`ReportAppService.SearchAsync(string term)` builds a query with `FromSqlRaw` by string
concatenation of `term`. Payload `'; DROP TABLE ...--` style input reaches the
database. Fix would use parameterised `FromSqlInterpolated` or LINQ.

### VL-002 — Stored XSS
A feedback form stores a message; the display page renders it with `@Html.Raw(...)`
instead of encoding. A `<script>` payload persists and executes for the next viewer.

### VL-003 — IDOR
`DocumentAppService.GetAsync(Guid id)` loads a document by id with no check that
`CreatorId == CurrentUser.Id`. Any authenticated user reads any other user's document.

### VL-004 — Path traversal
See the chain above. `DownloadAsync(string fileName)` does
`Path.Combine(basePath, fileName)` and returns the bytes, with no check that the
resolved path stays under `basePath`.

### VL-005 — SSRF
`WebhookAppService.TestAsync(string url)` issues an `HttpClient` GET to the
user-supplied `url` and returns the response body, allowing access to internal
endpoints and cloud metadata addresses.

### VL-006 — Hardcoded secrets & weak crypto
Real secrets sit in `appsettings.json` (kept intentionally, clearly fake values). A
`LegacyHasher` service hashes passwords with MD5. Target of the VL-004 chain.

### VL-007 — Missing authorization (ABP)
`AdminToolsAppService` carries a class-level `[Authorize]`, but `ResetDocumentsAsync`
adds `[AllowAnonymous]`, exposing a destructive operation (deletes all documents) to anyone.

### VL-008 — Tenant isolation bypass (ABP)
`ReportAppService.GetCrossTenantAsync` wraps its query in
`_dataFilter.Disable<IMultiTenant>()`, returning rows from **all** tenants to a
single-tenant caller.

### VL-009 — Permission declared but never enforced (ABP)
`AbpGoatPermissions.Documents.Delete` is defined and shown in the UI, but
`DocumentAppService.DeleteAsync` has **no** `[Authorize]` attribute and no manual
`CheckAsync`, so the permission is cosmetic.

### VL-010 — Host-only endpoint reachable by tenants (ABP)
`ListAllTenantsAsync` returns the full tenant list. It should be guarded by
`ICurrentTenant.Id == null` (host only) but performs no such check, leaking tenant
metadata to any tenant-scoped user.

### VL-011 — Excessive OpenIddict scope (ABP)
The DbMigrator seed over-provisions the **public** Swagger client (which has no secret):
it only needs the authorization-code flow, but is granted the password and
client-credentials grants as well, widening blast radius if the client is abused.

### VL-012 — Mass assignment
`UserProfileAppService.UpdateAsync` takes an `UpdateProfileDto` whose `RoleNames` list is
applied to the current user via `SetRolesAsync` with no authorization check, so a normal
user can grant themselves the admin role.

### VL-013 — Culture-sensitive `ToUpper()` auth bypass (.NET)
`IsPrivilegedAsync(string role)` compares with `role.ToUpper() == "ADMIN"` using the current
culture. Under the Turkish culture (`tr-TR`), `"admin".ToUpper()` yields `"ADMİN"`
(dotted capital I), so the comparison misbehaves and the check can be bypassed by
switching the request culture. This is why the app ships English **and** Turkish and
keeps the culture-switch endpoint enabled. Fix uses
`string.Equals(role, "Admin", StringComparison.OrdinalIgnoreCase)` or
`ToUpperInvariant()`.

---

## Notes for maintainers

- Every vulnerable type lives under a `Vulnerable` sub-namespace and, where practical,
  under `src/*/Vulnerable/` folders, so the intentional code is easy to find and
  exclude.
- Surrounding code still follows ABP and .NET conventions (one type per file,
  interfaces for services, thin module configuration). Only the specific insecure line
  is the defect — the scaffolding around it is realistic, so scanners face a real-world
  signal-to-noise ratio.
- Seed data creates two tenants (`tenant-a`, `tenant-b`) plus two ordinary users
  (`alice`, `bob`) in the host and in each tenant, each owning a document, so the
  tenant-isolation (VL-008) and IDOR (VL-003) findings are actually reproducible.
