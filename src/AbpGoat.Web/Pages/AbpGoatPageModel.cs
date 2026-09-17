using AbpGoat.Localization;
using Volo.Abp.AspNetCore.Mvc.UI.RazorPages;

namespace AbpGoat.Web.Pages;

public abstract class AbpGoatPageModel : AbpPageModel
{
    protected AbpGoatPageModel()
    {
        LocalizationResourceType = typeof(AbpGoatResource);
    }
}
