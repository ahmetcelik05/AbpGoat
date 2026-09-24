using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;
using Volo.Abp.Identity;

namespace AbpGoat.Vulnerable.Internal;

/// <summary>
/// Maintenance helpers meant to be called by background jobs and the ops console, not by end users.
/// </summary>
public class InternalMaintenanceAppService : ApplicationService
{
    private readonly IIdentityUserRepository _userRepository;

    public InternalMaintenanceAppService(IIdentityUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<List<string>> ExportUserEmailsAsync()
    {
        var users = await _userRepository.GetListAsync();
        return users.Select(u => u.Email).ToList();
    }
}
