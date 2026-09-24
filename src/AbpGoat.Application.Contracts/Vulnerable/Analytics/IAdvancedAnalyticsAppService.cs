using System.Threading.Tasks;
using Volo.Abp.Application.Services;

namespace AbpGoat.Vulnerable.Analytics;

public interface IAdvancedAnalyticsAppService : IApplicationService
{
    Task<string> GetAdvancedReportAsync();
}
