namespace clickdeal.Permissions;

public static class clickdealPermissions
{
    public const string GroupName = "clickdeal";

    //Add your own permission names. Example:
    //public const string MyPermission1 = GroupName + ".MyPermission1";

    public static class Roles
    {
        public const string User = GroupName + ".User";
        public const string Admin = GroupName + ".Admin";
    }
}
