using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;

namespace AbpGoat.Vulnerable.AdminTools;

public interface IAdminToolsAppService : IApplicationService
{
    Task ResetDocumentsAsync();

    Task<List<TenantInfoDto>> ListAllTenantsAsync();

    Task<bool> IsPrivilegedAsync(string role);

    Task<bool> IsPrivilegedSafeAsync(string role);
}
