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

        var documentsPermission = myGroup.AddPermission(AbpGoatPermissions.Documents.Default, L("Permission:Documents"));
        documentsPermission.AddChild(AbpGoatPermissions.Documents.Create, L("Permission:Documents.Create"));
        documentsPermission.AddChild(AbpGoatPermissions.Documents.Delete, L("Permission:Documents.Delete"));

        var notesPermission = myGroup.AddPermission(AbpGoatPermissions.Notes.Default, L("Permission:Notes"));
        notesPermission.AddChild(AbpGoatPermissions.Notes.Create, L("Permission:Notes.Create"));
        notesPermission.AddChild(AbpGoatPermissions.Notes.Edit, L("Permission:Notes.Edit"));
        notesPermission.AddChild(AbpGoatPermissions.Notes.Delete, L("Permission:Notes.Delete"));
        //Define your own permissions here. Example:
        //myGroup.AddPermission(AbpGoatPermissions.MyPermission1, L("Permission:MyPermission1"));
    }

    private static LocalizableString L(string name)
    {
        return LocalizableString.Create<AbpGoatResource>(name);
    }
}
