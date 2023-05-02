using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;

namespace clickdeal.Data;

/* This is used if database provider does't define
 * IclickdealDbSchemaMigrator implementation.
 */
public class NullclickdealDbSchemaMigrator : IclickdealDbSchemaMigrator, ITransientDependency
{
    public Task MigrateAsync()
    {
        return Task.CompletedTask;
    }
}
