using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;

namespace AbpGoat.Vulnerable.Reports;

public interface IReportAppService : IApplicationService
{
    Task<List<ReportItemDto>> SearchAsync(string term);

    Task<List<ReportItemDto>> GetCrossTenantAsync();
}
