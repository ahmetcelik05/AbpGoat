using System.Threading.Tasks;
using Volo.Abp.Application.Services;

namespace AbpGoat.Vulnerable.Profiles;

public interface IUserProfileAppService : IApplicationService
{
    Task UpdateAsync(UpdateProfileDto input);
}
