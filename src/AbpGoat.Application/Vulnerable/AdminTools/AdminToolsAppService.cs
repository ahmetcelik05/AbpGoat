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
/// Administrative utility operations used by the back office.
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

    [AllowAnonymous]
    public async Task ResetDocumentsAsync()
    {
        await _documentRepository.DeleteAsync(_ => true);
    }

    public async Task<List<TenantInfoDto>> ListAllTenantsAsync()
    {
        var tenants = await _tenantRepository.GetListAsync();
        return tenants
            .Select(t => new TenantInfoDto { Id = t.Id, Name = t.Name })
            .ToList();
    }

    public Task<bool> IsPrivilegedAsync(string role)
    {
        var normalized = role.ToUpper();
        return Task.FromResult(normalized == "ADMIN");
    }

    public Task<bool> IsPrivilegedSafeAsync(string role)
    {
        return Task.FromResult(string.Equals(role, "Admin", System.StringComparison.OrdinalIgnoreCase));
    }
}
