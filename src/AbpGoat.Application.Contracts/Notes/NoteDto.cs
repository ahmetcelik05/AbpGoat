using System;
using Volo.Abp.Application.Dtos;

namespace AbpGoat.Notes;

public class NoteDto : AuditedEntityDto<Guid>
{
    public string Title { get; set; } = string.Empty;

    public string Content { get; set; } = string.Empty;
}
