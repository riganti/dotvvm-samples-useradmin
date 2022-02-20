using System.Globalization;
using DotVVM.DynamicData.Helpers.Services;
using DotVVM.Framework.Controls.DynamicData.Annotations;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using DotVVM.Framework.Hosting;
using DotVVM.Framework.Routing;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Localization;
using Microsoft.EntityFrameworkCore;
using UserAdmin.Database;
using UserAdmin.Services.Roles;
using UserAdmin.Services.Users;

namespace UserAdmin;

public class Startup
{

    public IConfiguration Configuration { get; private set; }

    public Startup(IConfiguration configuration, IWebHostEnvironment env)
    {
        Configuration = configuration;
    }

    // This method gets called by the runtime. Use this method to add services to the container.
    // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddDataProtection();
        services.AddAuthorization();
        services.AddWebEncoders();
        services.AddAuthentication();

        services.AddDotVVM<DotvvmStartup>();

        services.AddEntityFrameworkSqlite()
            .AddDbContext<DbContext, AppDbContext>()
            .AddIdentity<IdentityUser, IdentityRole>()
            .AddEntityFrameworkStores<AppDbContext>();

        services.AddScoped<UserService>();
        services.AddScoped<UserDetailService>();
        services.AddScoped<RoleService>();
        services.AddScoped<ISelectorDataProvider<RoleSelectorItem>, RoleService>();

        services.AddRequestLocalization(options =>
        {
            var supportedCultures = new[]
            {
                new CultureInfo("cs"),
                new CultureInfo("en")
            };
            options.DefaultRequestCulture = new RequestCulture(culture: supportedCultures[0].TwoLetterISOLanguageName, supportedCultures[0].TwoLetterISOLanguageName);
            options.SupportedCultures = supportedCultures;
            options.SupportedUICultures = supportedCultures;
        });
    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        app.UseRequestLocalization();

        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }
        else
        {
            app.UseExceptionHandler("/error");
            app.UseHttpsRedirection();
            app.UseHsts();
        }

        app.UseRouting();

        app.UseAuthentication();
        app.UseAuthorization();

        // use DotVVM
        var dotvvmConfiguration = app.UseDotVVM<DotvvmStartup>(env.ContentRootPath);
        dotvvmConfiguration.AssertConfigurationIsValid();
            
        // use static files
        app.UseStaticFiles(new StaticFileOptions
        {
            FileProvider = new PhysicalFileProvider(env.WebRootPath)
        });

        app.UseEndpoints(endpoints => 
        {
            endpoints.MapDotvvmHotReload();

            // register ASP.NET Core MVC and other endpoint routing middlewares
        });
    }
}