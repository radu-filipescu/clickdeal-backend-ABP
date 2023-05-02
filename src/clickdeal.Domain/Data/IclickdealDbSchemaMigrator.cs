using System.Threading.Tasks;

namespace clickdeal.Data;

public interface IclickdealDbSchemaMigrator
{
    Task MigrateAsync();
}
