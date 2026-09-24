using System.Threading.Tasks;
using Volo.Abp.Application.Services;

namespace AbpGoat.Vulnerable.Webhooks;

public interface IWebhookAppService : IApplicationService
{
    Task<string> TestAsync(string url);

    Task<string> PingHealthAsync();
}
