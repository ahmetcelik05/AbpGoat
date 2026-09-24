using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using AbpGoat.Permissions;
using Volo.Abp.Application.Services;

namespace AbpGoat.Vulnerable.Webhooks;

/// <summary>
/// Webhook connectivity tester.
/// </summary>
[Authorize(AbpGoatPermissions.Documents.Default)]
public class WebhookAppService : ApplicationService, IWebhookAppService
{
    private static readonly HttpClient HttpClient = new();

    public async Task<string> TestAsync(string url)
    {
        var response = await HttpClient.GetAsync(url);
        return await response.Content.ReadAsStringAsync();
    }

    private const string HealthCheckUrl = "https://localhost:44394/health-status";

    public async Task<string> PingHealthAsync()
    {
        var response = await HttpClient.GetAsync(HealthCheckUrl);
        return await response.Content.ReadAsStringAsync();
    }
}
