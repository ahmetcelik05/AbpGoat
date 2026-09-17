using AbpGoat.Localization;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Localization;
using Volo.Abp.MultiTenancy;

namespace AbpGoat.Permissions;

public class AbpGoatPermissionDefinitionProvider : PermissionDefinitionProvider
{
    public override void Define(IPermissionDefinitionContext context)
    {
        var myGroup = context.AddGroup(AbpGoatPermissions.GroupName);

        var booksPermission = myGroup.AddPermission(AbpGoatPermissions.Books.Default, L("Permission:Books"));
        booksPermission.AddChild(AbpGoatPermissions.Books.Create, L("Permission:Books.Create"));
        booksPermission.AddChild(AbpGoatPermissions.Books.Edit, L("Permission:Books.Edit"));
        booksPermission.AddChild(AbpGoatPermissions.Books.Delete, L("Permission:Books.Delete"));

        var authorsPermission = myGroup.AddPermission(AbpGoatPermissions.Authors.Default, L("Permission:Authors"));
        authorsPermission.AddChild(AbpGoatPermissions.Authors.Create, L("Permission:Authors.Create"));
        authorsPermission.AddChild(AbpGoatPermissions.Authors.Edit, L("Permission:Authors.Edit"));
        authorsPermission.AddChild(AbpGoatPermissions.Authors.Delete, L("Permission:Authors.Delete"));
        //Define your own permissions here. Example:
        //myGroup.AddPermission(AbpGoatPermissions.MyPermission1, L("Permission:MyPermission1"));
    }

    private static LocalizableString L(string name)
    {
        return LocalizableString.Create<AbpGoatResource>(name);
    }
}
