using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using AbpGoat.Permissions;
using Volo.Abp.Application.Services;

namespace AbpGoat.Vulnerable.Webhooks;

/// <summary>
/// Webhook connectivity tester. Deliberately insecure — see VULNERABILITIES.md (VL-005).
/// </summary>
[Authorize(AbpGoatPermissions.Documents.Default)]
public class WebhookAppService : ApplicationService, IWebhookAppService
{
    private static readonly HttpClient HttpClient = new();

    // VL-005 (CWE-918, SSRF): the server fetches an arbitrary caller-supplied URL and returns
    // the body, so an attacker can reach internal services and cloud metadata endpoints.
    // A safe version would validate the host against an allow-list and block private ranges.
    public async Task<string> TestAsync(string url)
    {
        var response = await HttpClient.GetAsync(url);
        return await response.Content.ReadAsStringAsync();
    }
}
