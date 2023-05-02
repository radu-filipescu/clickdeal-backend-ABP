using Volo.Abp.Settings;

namespace clickdeal.Settings;

public class clickdealSettingDefinitionProvider : SettingDefinitionProvider
{
    public override void Define(ISettingDefinitionContext context)
    {
        //Define your own settings here. Example:
        //context.Add(new SettingDefinition(clickdealSettings.MySetting1));
    }
}
