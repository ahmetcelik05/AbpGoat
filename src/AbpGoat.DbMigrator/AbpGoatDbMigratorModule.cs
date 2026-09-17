using AbpGoat.EntityFrameworkCore;
using Volo.Abp.Autofac;
using Volo.Abp.Modularity;

namespace AbpGoat.DbMigrator;

[DependsOn(
    typeof(AbpAutofacModule),
    typeof(AbpGoatEntityFrameworkCoreModule),
    typeof(AbpGoatApplicationContractsModule)
)]
public class AbpGoatDbMigratorModule : AbpModule
{
}
