using Volo.Abp.Features;
using Volo.Abp.Validation.StringValues;

namespace AbpGoat.Vulnerable.Analytics;

public class AbpGoatFeatureDefinitionProvider : FeatureDefinitionProvider
{
    public override void Define(IFeatureDefinitionContext context)
    {
        var group = context.AddGroup(AbpGoatFeatures.GroupName);

        group.AddFeature(
            AbpGoatFeatures.AdvancedAnalytics,
            defaultValue: "false",
            valueType: new ToggleStringValueType());
    }
}
