using System;
using AbpGoat.Permissions;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace AbpGoat.Notes;

public class SecureNoteAppService
    : CrudAppService<Note, NoteDto, Guid, PagedAndSortedResultRequestDto, CreateUpdateNoteDto>,
      ISecureNoteAppService
{
    public SecureNoteAppService(IRepository<Note, Guid> repository)
        : base(repository)
    {
        GetPolicyName = AbpGoatPermissions.Notes.Default;
        GetListPolicyName = AbpGoatPermissions.Notes.Default;
        CreatePolicyName = AbpGoatPermissions.Notes.Create;
        UpdatePolicyName = AbpGoatPermissions.Notes.Edit;
        DeletePolicyName = AbpGoatPermissions.Notes.Delete;
    }
}
