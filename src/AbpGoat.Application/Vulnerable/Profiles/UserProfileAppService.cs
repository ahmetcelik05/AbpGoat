using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Volo.Abp.Application.Services;
using Volo.Abp.Data;
using Volo.Abp.Identity;
using Volo.Abp.Users;

namespace AbpGoat.Vulnerable.Profiles;

/// <summary>
/// Self-service profile update.
/// </summary>
[Authorize]
public class UserProfileAppService : ApplicationService, IUserProfileAppService
{
    private readonly IdentityUserManager _userManager;

    public UserProfileAppService(IdentityUserManager userManager)
    {
        _userManager = userManager;
    }

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

        foreach (var property in input.ExtraProperties)
        {
            user.SetProperty(property.Key, property.Value);
        }

        await _userManager.SetRolesAsync(user, input.RoleNames);
        await _userManager.UpdateAsync(user);
    }
}
