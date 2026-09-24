using System;
using Volo.Abp.Domain.Entities.Auditing;

namespace AbpGoat.Notes;

public class Note : AuditedAggregateRoot<Guid>
{
    public string Title { get; set; } = string.Empty;

    public string Content { get; set; } = string.Empty;
}
