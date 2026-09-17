using Volo.Abp.Settings;

namespace AbpGoat.Settings;

public class AbpGoatSettingDefinitionProvider : SettingDefinitionProvider
{
    public override void Define(ISettingDefinitionContext context)
    {
        //Define your own settings here. Example:
        //context.Add(new SettingDefinition(AbpGoatSettings.MySetting1));
    }
}
