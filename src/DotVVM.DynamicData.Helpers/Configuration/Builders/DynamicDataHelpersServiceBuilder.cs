using Microsoft.Extensions.DependencyInjection;

namespace DotVVM.DynamicData.Helpers.Configuration.Builders;

public abstract class DynamicDataHelpersServiceBuilder
{
    public abstract void Build(IServiceCollection services);
}