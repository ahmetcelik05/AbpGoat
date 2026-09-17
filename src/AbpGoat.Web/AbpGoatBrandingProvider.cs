using Volo.Abp.Ui.Branding;
using Volo.Abp.DependencyInjection;
using Microsoft.Extensions.Localization;
using AbpGoat.Localization;

namespace AbpGoat.Web;

[Dependency(ReplaceServices = true)]
public class AbpGoatBrandingProvider : DefaultBrandingProvider
{
    private IStringLocalizer<AbpGoatResource> _localizer;

    public AbpGoatBrandingProvider(IStringLocalizer<AbpGoatResource> localizer)
    {
        _localizer = localizer;
    }

    public override string AppName => _localizer["AppName"];
}
