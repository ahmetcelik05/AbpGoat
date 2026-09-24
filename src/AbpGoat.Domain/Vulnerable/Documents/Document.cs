using System;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.MultiTenancy;

namespace AbpGoat.Vulnerable.Documents;

/// <summary>
/// A user-owned document whose bytes are stored on the file system. Multi-tenant.
/// </summary>
public class Document : FullAuditedAggregateRoot<Guid>, IMultiTenant
{
    public string Title { get; private set; }

    /// <summary>Relative name of the file on disk, under the storage base path.</summary>
    public string FileName { get; private set; }

    public Guid OwnerUserId { get; private set; }

    public Guid? TenantId { get; private set; }

    protected Document()
    {
        Title = string.Empty;
        FileName = string.Empty;
    }

    public Document(Guid id, string title, string fileName, Guid ownerUserId, Guid? tenantId)
        : base(id)
    {
        Title = title;
        FileName = fileName;
        OwnerUserId = ownerUserId;
        TenantId = tenantId;
    }
}
