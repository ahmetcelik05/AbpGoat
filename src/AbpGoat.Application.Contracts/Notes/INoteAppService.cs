using System;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace AbpGoat.Notes;

public interface INoteAppService
    : ICrudAppService<NoteDto, Guid, PagedAndSortedResultRequestDto, CreateUpdateNoteDto>
{
}
