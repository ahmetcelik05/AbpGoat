using System;
using Volo.Abp.Application.Dtos;

namespace AbpGoat.Vulnerable.Documents;

public class DocumentDto : FullAuditedEntityDto<Guid>
{
    public string Title { get; set; } = string.Empty;

    public string FileName { get; set; } = string.Empty;

    public Guid OwnerUserId { get; set; }
}
