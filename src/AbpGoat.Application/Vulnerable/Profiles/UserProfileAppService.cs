using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Volo.Abp.Application.Services;
using Volo.Abp.Identity;
using Volo.Abp.Users;

namespace AbpGoat.Vulnerable.Profiles;

/// <summary>
/// Self-service profile update. Deliberately insecure — see VULNERABILITIES.md (VL-012).
/// </summary>
[Authorize]
public class UserProfileAppService : ApplicationService, IUserProfileAppService
{
    private readonly IdentityUserManager _userManager;

    public UserProfileAppService(IdentityUserManager userManager)
    {
        _userManager = userManager;
    }

    // VL-012 (CWE-915, mass assignment): RoleNames comes straight from the request body and is
    // applied to the current user with no authorization check, so a normal user can grant
    // themselves the admin role. Role changes must be a separate, permission-guarded operation.
    public async Task UpdateAsync(UpdateProfileDto input)
    {
        var user = await _userManager.GetByIdAsync(CurrentUser.GetId());

        if (input.Name != null)
        {
            user.Name = input.Name;
        }

        if (input.Surname != null)
        {
            user.Surname = input.Surname;
        }

        await _userManager.SetRolesAsync(user, input.RoleNames);
        await _userManager.UpdateAsync(user);
    }
}
