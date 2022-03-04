using DotVVM.DynamicData.Helpers.Services;
using Microsoft.Extensions.DependencyInjection;

namespace DotVVM.DynamicData.Helpers.Configuration.Builders;

public class DynamicDataHelpersListPageServiceBuilder<TModel, TFilter, TService> : DynamicDataHelpersServiceBuilder
    where TService : IListPageService<TModel, TFilter>
{
    public override void Build(IServiceCollection services)
    {
        services.AddScoped(typeof(IListPageService<TModel, TFilter>), typeof(TService));
    }
}