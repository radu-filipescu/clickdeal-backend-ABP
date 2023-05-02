using clickdeal.Localization;
using Volo.Abp.AspNetCore.Mvc.UI.RazorPages;

namespace clickdeal.Web.Pages;

/* Inherit your PageModel classes from this class.
 */
public abstract class clickdealPageModel : AbpPageModel
{
    protected clickdealPageModel()
    {
        LocalizationResourceType = typeof(clickdealResource);
    }
}
