using Microsoft.AspNetCore.Authorization;
using AbpGoat.Web.Pages;

namespace AbpGoat.Web.Pages.Vulnerable.Notice;

[AllowAnonymous]
public class IndexModel : AbpGoatPageModel
{
    public string Notice { get; private set; } = string.Empty;

    public void OnGet()
    {
        // A fixed, server-owned announcement. No request/user input reaches this string.
        Notice = "<strong>System notice:</strong> scheduled maintenance on Sunday 02:00–03:00 UTC.";
    }
}
