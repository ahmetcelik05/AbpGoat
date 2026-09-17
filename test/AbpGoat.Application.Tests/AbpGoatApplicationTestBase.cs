using Volo.Abp.Modularity;

namespace AbpGoat;

public abstract class AbpGoatApplicationTestBase<TStartupModule> : AbpGoatTestBase<TStartupModule>
    where TStartupModule : IAbpModule
{

}
