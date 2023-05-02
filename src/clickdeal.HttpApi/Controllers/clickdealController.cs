using clickdeal.Localization;
using Volo.Abp.AspNetCore.Mvc;

namespace clickdeal.Controllers;

/* Inherit your controllers from this class.
 */
public abstract class clickdealController : AbpControllerBase
{
    protected clickdealController()
    {
        LocalizationResource = typeof(clickdealResource);
    }
}
