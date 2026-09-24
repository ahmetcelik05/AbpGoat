using System;
using System.Threading.Tasks;
using AbpGoat.Vulnerable.Documents;
using AbpGoat.Vulnerable.Events;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.EventBus.Distributed;
using Volo.Abp.MultiTenancy;

namespace AbpGoat.Vulnerable.Events;

public class TenantReportEventHandler
    : IDistributedEventHandler<TenantReportEto>, ITransientDependency
{
    private readonly IRepository<Document, Guid> _documentRepository;
    private readonly ICurrentTenant _currentTenant;

    public ILogger<TenantReportEventHandler> Logger { get; set; }

    public TenantReportEventHandler(
        IRepository<Document, Guid> documentRepository,
        ICurrentTenant currentTenant)
    {
        _documentRepository = documentRepository;
        _currentTenant = currentTenant;
        Logger = NullLogger<TenantReportEventHandler>.Instance;
    }

    public async Task HandleEventAsync(TenantReportEto eventData)
    {
        using (_currentTenant.Change(eventData.TenantId))
        {
            var count = await _documentRepository.CountAsync();
            Logger.LogInformation("Tenant report: {Count} documents.", count);
        }
    }
}
