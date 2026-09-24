using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AbpGoat.Vulnerable.Reports;
using Volo.Abp.Application.Services;

namespace AbpGoat.Vulnerable.Tenancy;

public interface ITenantSwitchAppService : IApplicationService
{
    Task<List<ReportItemDto>> GetDocumentsForTenantAsync(Guid? tenantId);
}
