using clickdeal.Localization;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Localization;

namespace clickdeal.Permissions;

public class clickdealPermissionDefinitionProvider : PermissionDefinitionProvider
{
    public override void Define(IPermissionDefinitionContext context)
    {
        var myGroup = context.AddGroup(clickdealPermissions.GroupName);
        //Define your own permissions here. Example:
        //myGroup.AddPermission(clickdealPermissions.MyPermission1, L("Permission:MyPermission1"));
    }

    private static LocalizableString L(string name)
    {
        return LocalizableString.Create<clickdealResource>(name);
    }
}
