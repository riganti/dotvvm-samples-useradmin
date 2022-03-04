using System.Linq;
using DotVVM.DynamicData.Helpers;
using DotVVM.DynamicData.Helpers.Configuration;
using DotVVM.Framework.Configuration;
using DotVVM.Framework.Controls.DynamicData;
using DotVVM.Framework.ResourceManagement;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using UserAdmin.Database;
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
                        .AddListPage<UserListModel, UserFilterModel>(page => page 
                            .UseEfCoreService<AppDbContext, AppUser, UserListModel>(service => service
                                .UseProjection(users => users.Select(u => new UserListModel()
                                {
                                    Id = u.Id,
                                    UserName = u.UserName,
                                    Email = u.Email,
                                    PhoneNumber = u.PhoneNumber,
                                    Roles = string.Join(", ", u.Roles.Select(r => r.Role.Name))
                                }))
                            )
                        )
                        .AddDetailPage<UserDetailModel, string>(page => page
                            .AddSelector<RoleSelectorItem>()
                            .UseEfCoreService<AppDbContext, AppUser, UserDetailModel, string>(service => service
                                .UseEntityFilter(users => users.Include(u => u.Roles))
                                .UseMapping(e => new UserDetailModel()
                                {
                                    UserName = e.UserName,
                                    Email = e.Email, 
                                    PhoneNumber = e.PhoneNumber,
                                    Roles = e.Roles.Select(r => r.RoleId).ToList()
                                }, 
                                (e, m) =>
                                {
                                    e.UserName = m.UserName;
                                    e.Email = m.Email;
                                    e.PhoneNumber = m.PhoneNumber;
                                    e.Roles.Clear();
                                    foreach (var r in m.Roles)
                                    {
                                        e.Roles.Add(new AppUserRole() { RoleId = r });
                                    }
                                })
                            )
                        )
                    )
                    .AddSection("Roles", section => section
                        .AddListPage<RoleModel, RoleFilterModel, RoleService>()
                        .AddDetailPage<RoleModel, string, RoleService>()
                    )
                    .AddSelector<RoleSelectorItem, RoleService>()
                    .UseResourceFile(typeof(PageNames));
            });
    }
}