using System;
using System.Threading.Tasks;
using AbpGoat.Vulnerable.Documents;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using AbpGoat.Web.Pages;
using Volo.Abp.Domain.Repositories;

namespace AbpGoat.Web.Pages.Vulnerable.Danger;

[Authorize]
public class IndexModel : AbpGoatPageModel
{
    private readonly IRepository<Document, Guid> _documentRepository;

    public IndexModel(IRepository<Document, Guid> documentRepository)
    {
        _documentRepository = documentRepository;
    }

    public void OnGet()
    {
    }

    [IgnoreAntiforgeryToken]
    public async Task<IActionResult> OnPostPurgeAsync()
    {
        await _documentRepository.DeleteAsync(_ => true);
        return RedirectToPage();
    }
}
