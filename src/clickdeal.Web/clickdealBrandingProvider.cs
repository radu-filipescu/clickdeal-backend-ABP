using Volo.Abp.Ui.Branding;
using Volo.Abp.DependencyInjection;

namespace clickdeal.Web;

[Dependency(ReplaceServices = true)]
public class clickdealBrandingProvider : DefaultBrandingProvider
{
    public override string AppName => "clickdeal";
}
