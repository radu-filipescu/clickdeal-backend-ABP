using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using clickdeal.Data;
using Volo.Abp.DependencyInjection;

namespace clickdeal.EntityFrameworkCore;

public class EntityFrameworkCoreclickdealDbSchemaMigrator
    : IclickdealDbSchemaMigrator, ITransientDependency
{
    private readonly IServiceProvider _serviceProvider;

    public EntityFrameworkCoreclickdealDbSchemaMigrator(
        IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public async Task MigrateAsync()
    {
        /* We intentionally resolving the clickdealDbContext
         * from IServiceProvider (instead of directly injecting it)
         * to properly get the connection string of the current tenant in the
         * current scope.
         */

        await _serviceProvider
            .GetRequiredService<clickdealDbContext>()
            .Database
            .MigrateAsync();
    }
}
