# AbpGoat

> ## ⚠️ DELIBERATELY VULNERABLE APPLICATION — DO NOT DEPLOY
>
> AbpGoat is an **intentionally insecure** ABP (ASP.NET Core) application. It exists only
> as a practice target for security tooling: static analyzers (SAST), dynamic scanners
> (DAST), and AI-driven penetration-testing agents.
>
> - **Do not** deploy it to a public, shared, or internet-facing environment.
> - Run it on **localhost only**, ideally inside a disposable VM or container.
> - The credentials and secrets in this repository are **fake** and exist only so the
>   application runs locally out of the box.

## What this is

AbpGoat is a normal-looking, layered ABP application — a small "book store" plus a few
extra features — into which a catalogue of security defects has been planted on purpose.
It is the ABP-flavoured equivalent of WebGoat or Juice Shop.

The flaws are in the **sample application code**, not in the ABP Framework itself. Every
intentional defect lives under an `AbpGoat.*.Vulnerable` namespace so it stays clearly
separated from the framework's own code and from the realistic scaffolding around it.

The defects deliberately span three detection layers:

- **Generic web flaws** — SQL injection, stored XSS, IDOR, path traversal, SSRF,
  hardcoded secrets, weak crypto, mass assignment.
- **ABP-specific flaws** — a missing `[Authorize]`, a disabled multi-tenancy data filter,
  a permission that is declared but never checked, a host-only endpoint reachable by
  tenants, and an over-provisioned OpenIddict client.
- **.NET-specific flaws** — a culture-sensitive `ToUpper()` used in an authorization
  decision (the Turkish "i" problem).

The full list, with CWE ids, exact locations, an exploit chain, and a scoring sheet for
comparing tools, is in **[VULNERABILITIES.md](VULNERABILITIES.md)**.

## Tech stack

- .NET 10, ABP Framework, layered (DDD) solution
- ASP.NET Core MVC / Razor Pages UI (LeptonX Lite theme)
- Entity Framework Core on PostgreSQL
- OpenIddict authentication, multi-tenancy enabled
- File-system BLOB storage

## Running locally

### Prerequisites

- [.NET 10 SDK](https://dotnet.microsoft.com/download/dotnet)
- [Node.js 18 or 20](https://nodejs.org/en) (for client-side libraries)
- A local PostgreSQL instance

### Steps

```bash
# 1. Restore client-side libraries (only needed after a fresh clone).
abp install-libs

# 2. Check the "Default" connection string in the appsettings.json files under
#    src/AbpGoat.Web and src/AbpGoat.DbMigrator, and adjust it for your PostgreSQL.

# 3. Create the database schema and seed demo data (tenants, users, documents).
dotnet run --project src/AbpGoat.DbMigrator

# 4. Run the web application.
dotnet run --project src/AbpGoat.Web
```

The application listens on `https://localhost:44394` by default.

## Accounts and seed data

The database migrator seeds a multi-tenant data set so that the ownership and tenant
isolation flaws are reproducible against real, multi-owner data. All accounts use the
password `1q2w3E*`.

| Context | Users |
|---------|-------|
| Host | `admin` (default), `alice`, `bob` |
| Tenant `tenant-a` | `alice`, `bob` |
| Tenant `tenant-b` | `alice`, `bob` |

`alice` and `bob` are ordinary, non-admin users, each owning a private document. Use them
to reproduce the IDOR (VL-003) and tenant-isolation (VL-008) findings. To switch tenant at
login, use the tenant switch on the login screen.

## Solution structure

- `src/AbpGoat.Domain` / `AbpGoat.Domain.Shared` — entities, domain services, constants.
- `src/AbpGoat.Application` / `AbpGoat.Application.Contracts` — application services and DTOs.
- `src/AbpGoat.EntityFrameworkCore` — EF Core mappings, migrations, custom repositories.
- `src/AbpGoat.HttpApi` — auto-generated API controllers.
- `src/AbpGoat.Web` — MVC / Razor Pages host and UI.
- `src/AbpGoat.DbMigrator` — console app that applies migrations and seeds data.
- `test/` — application, domain, and EF Core test projects.

Application services under `AbpGoat.Vulnerable.*` are automatically exposed as REST
endpoints under `/api/app/...`, so DAST and agent-based tools can reach them without any
extra wiring.

## Notes

- **Signing certificate.** OpenIddict expects an `openiddict.pfx` file. A development
  certificate is generated automatically for local runs. To create one manually:

  ```bash
  dotnet dev-certs https -v -ep openiddict.pfx -p fa73fb05-7b12-4a03-918a-36217ddf872f
  ```

- **Adding vulnerabilities.** Keep new intentional defects under an `AbpGoat.*.Vulnerable`
  namespace, and record each one in [VULNERABILITIES.md](VULNERABILITIES.md) so the ground
  truth stays complete.

## Disclaimer

This project is provided for security education, tooling evaluation, and authorized
testing only. Running it exposes real, exploitable vulnerabilities by design. You are
responsible for keeping it isolated. Do not use it against systems or networks you do not
own or are not authorized to test.
