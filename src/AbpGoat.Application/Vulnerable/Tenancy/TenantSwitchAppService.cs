using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using AbpGoat.Permissions;
using AbpGoat.Vulnerable.Documents;
using AbpGoat.Vulnerable.Reports;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.MultiTenancy;

namespace AbpGoat.Vulnerable.Tenancy;

[Authorize(AbpGoatPermissions.Documents.Default)]
public class TenantSwitchAppService : ApplicationService, ITenantSwitchAppService
{
    private readonly IRepository<Document, Guid> _documentRepository;
    private readonly ICurrentTenant _currentTenant;

    public TenantSwitchAppService(
        IRepository<Document, Guid> documentRepository,
        ICurrentTenant currentTenant)
    {
        _documentRepository = documentRepository;
        _currentTenant = currentTenant;
    }

    public async Task<List<ReportItemDto>> GetDocumentsForTenantAsync(Guid? tenantId)
    {
        using (_currentTenant.Change(tenantId))
        {
            var documents = await _documentRepository.GetListAsync();
            return documents
                .Select(d => new ReportItemDto
                {
                    Id = d.Id,
                    Title = d.Title,
                    OwnerUserId = d.OwnerUserId,
                    TenantId = d.TenantId
                })
                .ToList();
        }
    }
}
