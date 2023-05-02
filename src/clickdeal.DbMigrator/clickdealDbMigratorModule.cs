using clickdeal.EntityFrameworkCore;
using Volo.Abp.Autofac;
using Volo.Abp.Modularity;

namespace clickdeal.DbMigrator;

[DependsOn(
    typeof(AbpAutofacModule),
    typeof(clickdealEntityFrameworkCoreModule),
    typeof(clickdealApplicationContractsModule)
    )]
public class clickdealDbMigratorModule : AbpModule
{

}
