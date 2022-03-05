using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DotVVM.DynamicData.Helpers.Configuration.Builders;
using DotVVM.DynamicData.Helpers.Context;
using DotVVM.DynamicData.Helpers.Hosting;
using DotVVM.DynamicData.Helpers.Metadata;
using DotVVM.Framework.Hosting;
using Microsoft.Extensions.DependencyInjection;

namespace DotVVM.DynamicData.Helpers.Configuration
{
    public static class DotvvmServiceCollectionExtensions
    {

        public static void AddDynamicDataHelpers(this IDotvvmServiceCollection services, 
            string masterPagePath,
            Type viewModelHostType,
            Action<DynamicDataHelpersConfigurationBuilder>? configure = null)
        {
            services.Services.AddSingleton<IPageMetadataProvider, PageMetadataProvider>();

            services.Services.Configure<AggregateMarkupFileLoaderOptions>(options =>
            {
                options.LoaderTypes.Insert(0, typeof(DynamicDataHelpersMarkupFileLoader));
            });
            services.Services.AddSingleton<DynamicDataHelpersMarkupFileLoader>();

            services.Services.AddScoped<ServiceContext>(provider =>
            {
                var context = provider.GetRequiredService<IDotvvmRequestContext>();
                return new ServiceContext(context, provider.GetRequiredService<IPageMetadataProvider>().GetPageConfiguration(context));
            });

            var options = new DynamicDataHelpersConfigurationBuilder(masterPagePath, viewModelHostType);
            configure?.Invoke(options);
            var configuration = options.Build(services.Services);
            
            services.Services.AddSingleton(configuration);
        }

    }
}
