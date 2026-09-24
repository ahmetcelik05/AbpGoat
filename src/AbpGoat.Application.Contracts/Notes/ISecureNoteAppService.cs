using System;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace AbpGoat.Notes;

public interface ISecureNoteAppService
    : ICrudAppService<NoteDto, Guid, PagedAndSortedResultRequestDto, CreateUpdateNoteDto>
{
}
