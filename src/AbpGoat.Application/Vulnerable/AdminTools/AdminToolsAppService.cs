using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using AbpGoat.Vulnerable.Documents;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.MultiTenancy;
using Volo.Abp.TenantManagement;

namespace AbpGoat.Vulnerable.AdminTools;

/// <summary>
/// "Admin" utilities. Deliberately insecure — see VULNERABILITIES.md (VL-007, VL-010, VL-013).
/// The class requires authentication, but that is not the same as authorization.
/// </summary>
[Authorize]
public class AdminToolsAppService : ApplicationService, IAdminToolsAppService
{
    private readonly IRepository<Document, System.Guid> _documentRepository;
    private readonly ITenantRepository _tenantRepository;
    private readonly ICurrentTenant _currentTenant;

    public AdminToolsAppService(
        IRepository<Document, System.Guid> documentRepository,
        ITenantRepository tenantRepository,
        ICurrentTenant currentTenant)
    {
        _documentRepository = documentRepository;
        _tenantRepository = tenantRepository;
        _currentTenant = currentTenant;
    }

    // VL-007 (CWE-862): a destructive operation on a class marked [Authorize] is opened up
    // with [AllowAnonymous], so anyone — signed in or not — can wipe all documents.
    [AllowAnonymous]
    public async Task ResetDocumentsAsync()
    {
        await _documentRepository.DeleteAsync(_ => true);
    }

    // VL-010 (CWE-266): listing every tenant is a host-only operation, but there is no
    // check that the caller is the host (ICurrentTenant.Id == null). A tenant-scoped user
    // can enumerate all tenants in the system.
    public async Task<List<TenantInfoDto>> ListAllTenantsAsync()
    {
        var tenants = await _tenantRepository.GetListAsync();
        return tenants
            .Select(t => new TenantInfoDto { Id = t.Id, Name = t.Name })
            .ToList();
    }

    // VL-013 (CWE-178): the privilege decision uppercases with the current culture. Under
    // the Turkish culture (tr-TR) "admin".ToUpper() is "ADMİN" (dotted capital I), so the
    // comparison is culture-dependent and can be desynchronised by switching the request
    // culture. A culture-invariant comparison must be used for security decisions.
    public Task<bool> IsPrivilegedAsync(string role)
    {
        var normalized = role.ToUpper();
        return Task.FromResult(normalized == "ADMIN");
    }
}
