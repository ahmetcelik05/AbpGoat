using Volo.Abp.Modularity;

namespace AbpGoat;

/* Inherit from this class for your domain layer tests. */
public abstract class AbpGoatDomainTestBase<TStartupModule> : AbpGoatTestBase<TStartupModule>
    where TStartupModule : IAbpModule
{

}
