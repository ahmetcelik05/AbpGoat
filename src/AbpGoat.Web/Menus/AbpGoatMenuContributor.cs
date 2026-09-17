using System.Threading.Tasks;
using AbpGoat.Localization;
using AbpGoat.Permissions;
using AbpGoat.MultiTenancy;
using Volo.Abp.SettingManagement.Web.Navigation;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Identity.Web.Navigation;
using Volo.Abp.UI.Navigation;
using Volo.Abp.TenantManagement.Web.Navigation;
namespace AbpGoat.Web.Menus;
public class AbpGoatMenuContributor : IMenuContributor
{
    public async Task ConfigureMenuAsync(MenuConfigurationContext context)
    {
        if (context.Menu.Name == StandardMenus.Main)
        {
            await ConfigureMainMenuAsync(context);
        }
    }
    private static Task ConfigureMainMenuAsync(MenuConfigurationContext context)
    {
        var l = context.GetLocalizer<AbpGoatResource>();
        //Home
        context.Menu.AddItem(
            new ApplicationMenuItem(
                AbpGoatMenus.Home,
                l["Menu:Home"],
                "~/",
                icon: "fa fa-home",
                order: 1
            )
        );
        //Administration
        var administration = context.Menu.GetAdministration();
        administration.Order = 6;
        //Administration->Identity
        administration.SetSubItemOrder(IdentityMenuNames.GroupName, 1);

        if (MultiTenancyConsts.IsEnabled)
        {
            administration.SetSubItemOrder(TenantManagementMenuNames.GroupName, 1);
        }
        else
        {
            administration.TryRemoveMenuItem(TenantManagementMenuNames.GroupName);
        }

        administration.SetSubItemOrder(SettingManagementMenuNames.GroupName, 3);
        //Administration->Settings
        administration.SetSubItemOrder(SettingManagementMenuNames.GroupName, 8);

        context.Menu.AddItem(
            new ApplicationMenuItem(
                "BooksStore",
                l["Menu:AbpGoat"],
                icon: "fa fa-book"
            ).AddItem(
                new ApplicationMenuItem(
                    "BooksStore.Books",
                    l["Menu:Books"],
                    url: "/Books"
                ).RequirePermissions(AbpGoatPermissions.Books.Default)
            ).AddItem(
                new ApplicationMenuItem(
                    "BooksStore.Authors",
                    l["Menu:Authors"],
                    url: "/Authors"
                ).RequirePermissions(AbpGoatPermissions.Authors.Default)
            ).AddItem(
                new ApplicationMenuItem(
                    "BooksStore.Feedback",
                    l["Menu:Feedback"],
                    url: "/Vulnerable/Feedback"
                )
            )
        );

        return Task.CompletedTask;
    }
}
