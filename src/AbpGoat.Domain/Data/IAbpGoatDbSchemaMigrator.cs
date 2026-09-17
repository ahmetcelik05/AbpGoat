using System.Threading.Tasks;

namespace AbpGoat.Data;

public interface IAbpGoatDbSchemaMigrator
{
    Task MigrateAsync();
}
