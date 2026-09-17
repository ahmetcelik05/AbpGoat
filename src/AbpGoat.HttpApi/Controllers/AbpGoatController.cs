using AbpGoat.Localization;
using Volo.Abp.AspNetCore.Mvc;

namespace AbpGoat.Controllers;

/* Inherit your controllers from this class.
 */
public abstract class AbpGoatController : AbpControllerBase
{
    protected AbpGoatController()
    {
        LocalizationResource = typeof(AbpGoatResource);
    }
}
