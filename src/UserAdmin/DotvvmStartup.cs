using DotVVM.DynamicData.Helpers.Configuration;
using DotVVM.Framework.Configuration;
using DotVVM.Framework.Controls.DynamicData;
using DotVVM.Framework.ResourceManagement;
using Microsoft.Extensions.DependencyInjection;
using UserAdmin.Resources;
using UserAdmin.Services.Roles;
using UserAdmin.Services.Users;
using UserAdmin.ViewModels;

namespace UserAdmin;

public class DotvvmStartup : IDotvvmStartup, IDotvvmServiceConfigurator
{
    // For more information about this class, visit https://dotvvm.com/docs/tutorials/basics-project-structure
    public void Configure(DotvvmConfiguration config, string applicationPath)
    {
        ConfigureRoutes(config, applicationPath);
        ConfigureControls(config, applicationPath);
        ConfigureResources(config, applicationPath);
    }

    private void ConfigureRoutes(DotvvmConfiguration config, string applicationPath)
    {
        config.RouteTable.Add("Default", "", "Views/Default.dothtml");
    }

    private void ConfigureControls(DotvvmConfiguration config, string applicationPath)
    {
        // register code-only controls and markup controls
    }

    private void ConfigureResources(DotvvmConfiguration config, string applicationPath)
    {
        // register custom resources and adjust paths to the built-in resources
        config.Resources.RegisterScriptFile("bootstrap", "wwwroot/lib/bootstrap/js/bootstrap.min.js",
            dependencies: new[] { "bootstrap-css", "jquery" });
        config.Resources.RegisterStylesheetFile("bootstrap-css", "wwwroot/lib/bootstrap/css/bootstrap.min.css");
        config.Resources.RegisterScriptFile("jquery", "wwwroot/lib/jquery/jquery.min.js");
    }

    public void ConfigureServices(IDotvvmServiceCollection options)
    {
        options.AddDefaultTempStorages("temp");
        options.AddHotReload();

        options.AddDynamicData(options =>
        {
            options.PropertyDisplayNamesResourceFile = typeof(PropertyNames);
        });

        options.AddDynamicDataHelpers(
            masterPagePath: "Views/MasterPage.dotmaster",
            viewModelHostType: typeof(DynamicPageViewModelHost<,>),
            config =>
            {
                config
                    .AddSection("Users", section => section
                        .AddListPage<UserListModel, UserFilterModel, UserService>()
                        .AddDetailPage<UserDetailModel, string, UserDetailService>(page => page.AddSelector<RoleSelectorItem>())
                    )
                    .AddSection("Roles", section => section
                        .AddListPage<RoleModel, RoleFilterModel, RoleService>()
                        .AddDetailPage<RoleModel, string, RoleService>()
                    )
                    .AddSelector<RoleSelectorItem, RoleService>()
                    .UseResourceFile(typeof(PageNames));
            });
    }

    //config
    //  .AddSection("Users", section => section
    //    .AddListPage<UserListModel>(page => page
    //      .UseFilter<UserFilterModel>((filter, query) => query.Where(i => i.Name.Contains(filter.SearchText)))
    //      .UseEntityFrameworkCoreStore<AppDbContext>(c => c.Users, store => store
    //        .UseMapping(entity => new UserFilterModel() { ... }, model => new IdentityUser() { ... })
    //        //.UseAutoMapper()
    //      )
    //      .AddUserClaimFilter("CompanyIdClaim", (claimValue, query) => query.Where(i => i.CompanyId == claimValue))
    //    )
    //    .AddDetailPage<UserDetailModel>(page => page
    //      .UseEntityFrameworkCoreStore<AppDbContext>(c => c.Users, store => store
    //        .UseAutoMapper()
    //        .UseSoftDelete(item => item.IsDeleted)
    //      )
    //      .AddUserRoleFilter("admin")
    //      .AddUserClaimFilter("CompanyIdClaim", (claimValue, item) => item.CompanyId = claimValue))
    //      .EnableInsert(false)
    //  )
}