using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AbpGoat.Vulnerable.Feedbacks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using AbpGoat.Web.Pages;
using Volo.Abp.Domain.Repositories;

namespace AbpGoat.Web.Pages.Vulnerable.Feedback;

[AllowAnonymous]
public class IndexModel : AbpGoatPageModel
{
    private readonly IRepository<AbpGoat.Vulnerable.Feedbacks.Feedback, Guid> _feedbackRepository;

    public IndexModel(IRepository<AbpGoat.Vulnerable.Feedbacks.Feedback, Guid> feedbackRepository)
    {
        _feedbackRepository = feedbackRepository;
    }

    public List<AbpGoat.Vulnerable.Feedbacks.Feedback> Feedbacks { get; set; } = new();

    [BindProperty]
    public string? AuthorName { get; set; }

    [BindProperty]
    public string? Message { get; set; }

    public async Task OnGetAsync()
    {
        Feedbacks = await _feedbackRepository.GetListAsync();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!string.IsNullOrWhiteSpace(Message))
        {
            await _feedbackRepository.InsertAsync(
                new AbpGoat.Vulnerable.Feedbacks.Feedback(
                    GuidGenerator.Create(),
                    string.IsNullOrWhiteSpace(AuthorName) ? "Anonymous" : AuthorName!,
                    Message!),
                autoSave: true);
        }

        return RedirectToPage();
    }
}
