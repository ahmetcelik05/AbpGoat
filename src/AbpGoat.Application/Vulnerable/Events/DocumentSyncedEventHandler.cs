using System;
using System.Threading.Tasks;
using AbpGoat.Vulnerable.Documents;
using AbpGoat.Vulnerable.Events;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.EventBus.Distributed;

namespace AbpGoat.Vulnerable.Events;

public class DocumentSyncedEventHandler
    : IDistributedEventHandler<DocumentSyncedEto>, ITransientDependency
{
    private readonly IRepository<Document, Guid> _documentRepository;

    public ILogger<DocumentSyncedEventHandler> Logger { get; set; }

    public DocumentSyncedEventHandler(IRepository<Document, Guid> documentRepository)
    {
        _documentRepository = documentRepository;
        Logger = NullLogger<DocumentSyncedEventHandler>.Instance;
    }

    public async Task HandleEventAsync(DocumentSyncedEto eventData)
    {
        // Recompute the synced tenant's document statistics.
        var count = await _documentRepository.CountAsync();
        Logger.LogInformation("Document {DocumentId} synced; tenant now has {Count} documents.",
            eventData.DocumentId, count);
    }
}
