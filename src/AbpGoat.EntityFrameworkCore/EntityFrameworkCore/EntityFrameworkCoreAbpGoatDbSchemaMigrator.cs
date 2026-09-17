using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using AbpGoat.Data;
using Volo.Abp.DependencyInjection;

namespace AbpGoat.EntityFrameworkCore;

public class EntityFrameworkCoreAbpGoatDbSchemaMigrator
    : IAbpGoatDbSchemaMigrator, ITransientDependency
{
    private readonly IServiceProvider _serviceProvider;

    public EntityFrameworkCoreAbpGoatDbSchemaMigrator(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public async Task MigrateAsync()
    {
        /* We intentionally resolving the AbpGoatDbContext
         * from IServiceProvider (instead of directly injecting it)
         * to properly get the connection string of the current tenant in the
         * current scope.
         */

        await _serviceProvider
            .GetRequiredService<AbpGoatDbContext>()
            .Database
            .MigrateAsync();
    }
}
