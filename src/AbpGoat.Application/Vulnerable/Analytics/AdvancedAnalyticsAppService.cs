using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Volo.Abp.Application.Services;

namespace AbpGoat.Vulnerable.Analytics;

[Authorize]
public class AdvancedAnalyticsAppService : ApplicationService, IAdvancedAnalyticsAppService
{
    public Task<string> GetAdvancedReportAsync()
    {
        return Task.FromResult("Advanced analytics: 1,240 active users, 87% retention, churn 3.1%.");
    }
}
