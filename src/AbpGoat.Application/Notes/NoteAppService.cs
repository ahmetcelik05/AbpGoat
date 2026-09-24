using System;
using AbpGoat.Permissions;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace AbpGoat.Notes;

public class NoteAppService
    : CrudAppService<Note, NoteDto, Guid, PagedAndSortedResultRequestDto, CreateUpdateNoteDto>,
      INoteAppService
{
    public NoteAppService(IRepository<Note, Guid> repository)
        : base(repository)
    {
        GetPolicyName = AbpGoatPermissions.Notes.Default;
        GetListPolicyName = AbpGoatPermissions.Notes.Default;
        CreatePolicyName = AbpGoatPermissions.Notes.Create;
        UpdatePolicyName = AbpGoatPermissions.Notes.Edit;
    }
}
