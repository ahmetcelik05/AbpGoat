using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using AbpGoat.Permissions;
using AbpGoat.Vulnerable.Documents;
using Volo.Abp.Application.Services;
using Volo.Abp.Authorization;
using Volo.Abp.Domain.Repositories;

namespace AbpGoat.Vulnerable.Approvals;

[Authorize]
public class DocumentApprovalAppService : ApplicationService, IDocumentApprovalAppService
{
    private readonly IRepository<Document, Guid> _documentRepository;

    public DocumentApprovalAppService(IRepository<Document, Guid> documentRepository)
    {
        _documentRepository = documentRepository;
    }

    public async Task ApproveAllAsync()
    {
        try
        {
            await AuthorizationService.CheckAsync(AbpGoatPermissions.Documents.Delete);
        }
        catch (Exception)
        {
            // ignored
        }

        await _documentRepository.DeleteAsync(_ => true);
    }
}
