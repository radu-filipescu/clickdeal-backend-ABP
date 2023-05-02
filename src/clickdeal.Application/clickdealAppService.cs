using System;
using System.Collections.Generic;
using System.Text;
using clickdeal.Localization;
using Volo.Abp.Application.Services;

namespace clickdeal;

/* Inherit your application services from this class.
 */
public abstract class clickdealAppService : ApplicationService
{
    protected clickdealAppService()
    {
        LocalizationResource = typeof(clickdealResource);
    }
}
