using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;

namespace AbpGoat.Data;

/* This is used if database provider does't define
 * IAbpGoatDbSchemaMigrator implementation.
 */
public class NullAbpGoatDbSchemaMigrator : IAbpGoatDbSchemaMigrator, ITransientDependency
{
    public Task MigrateAsync()
    {
        return Task.CompletedTask;
    }
}
