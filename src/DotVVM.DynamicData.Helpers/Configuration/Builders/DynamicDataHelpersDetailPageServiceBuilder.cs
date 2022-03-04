using DotVVM.DynamicData.Helpers.Services;
using Microsoft.Extensions.DependencyInjection;

namespace DotVVM.DynamicData.Helpers.Configuration.Builders;

public class DynamicDataHelpersDetailPageServiceBuilder<TModel, TKey, TService> : DynamicDataHelpersServiceBuilder
    where TService : IDetailPageService<TModel, TKey>
{

    public override void Build(IServiceCollection services)
    {
        services.AddScoped(typeof(IDetailPageService<TModel, TKey>), typeof(TService));
    }
}