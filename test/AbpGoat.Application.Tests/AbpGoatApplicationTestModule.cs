using Volo.Abp.Modularity;

namespace AbpGoat;

[DependsOn(
    typeof(AbpGoatApplicationModule),
    typeof(AbpGoatDomainTestModule)
)]
public class AbpGoatApplicationTestModule : AbpModule
{

}
