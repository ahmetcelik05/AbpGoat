using AbpGoat.Localization;
using Volo.Abp.Application.Services;

namespace AbpGoat;

/* Inherit your application services from this class.
 */
public abstract class AbpGoatAppService : ApplicationService
{
    protected AbpGoatAppService()
    {
        LocalizationResource = typeof(AbpGoatResource);
    }
}
