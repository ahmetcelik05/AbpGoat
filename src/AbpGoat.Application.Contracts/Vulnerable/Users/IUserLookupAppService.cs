using System;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;

namespace AbpGoat.Vulnerable.Users;

public interface IUserLookupAppService : IApplicationService
{
    Task<UserDetailsDto> GetAsync(Guid id);
}
