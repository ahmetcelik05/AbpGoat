using Volo.Abp.Modularity;

namespace AbpGoat;

[DependsOn(
    typeof(AbpGoatDomainModule),
    typeof(AbpGoatTestBaseModule)
)]
public class AbpGoatDomainTestModule : AbpModule
{

}
