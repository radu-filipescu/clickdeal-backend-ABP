using Volo.Abp.Modularity;

namespace clickdeal;

[DependsOn(
    typeof(clickdealApplicationModule),
    typeof(clickdealDomainTestModule)
    )]
public class clickdealApplicationTestModule : AbpModule
{

}
