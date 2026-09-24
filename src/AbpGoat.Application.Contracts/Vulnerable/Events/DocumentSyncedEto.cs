using System;

namespace AbpGoat.Vulnerable.Events;

[Serializable]
public class DocumentSyncedEto
{
    public Guid DocumentId { get; set; }

    public Guid? TenantId { get; set; }
}
