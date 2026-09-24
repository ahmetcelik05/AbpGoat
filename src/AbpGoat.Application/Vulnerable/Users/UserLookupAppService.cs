using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Volo.Abp.Application.Services;
using Volo.Abp.Identity;

namespace AbpGoat.Vulnerable.Users;

[Authorize]
public class UserLookupAppService : ApplicationService, IUserLookupAppService
{
    private readonly IdentityUserManager _userManager;

    public UserLookupAppService(IdentityUserManager userManager)
    {
        _userManager = userManager;
    }

    public async Task<UserDetailsDto> GetAsync(Guid id)
    {
        var user = await _userManager.GetByIdAsync(id);

        return new UserDetailsDto
        {
            Id = user.Id,
            UserName = user.UserName,
            Email = user.Email,
            PasswordHash = user.PasswordHash
        };
    }
}
