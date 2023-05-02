using clickdeal.EntityFrameworkCore;
using Volo.Abp.Modularity;

namespace clickdeal;

[DependsOn(
    typeof(clickdealEntityFrameworkCoreTestModule)
    )]
public class clickdealDomainTestModule : AbpModule
{

}
