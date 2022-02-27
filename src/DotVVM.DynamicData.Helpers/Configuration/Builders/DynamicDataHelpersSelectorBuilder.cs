using DotVVM.Framework.Controls.DynamicData.Annotations;
using Microsoft.Extensions.DependencyInjection;

namespace DotVVM.DynamicData.Helpers.Configuration.Builders;

public class DynamicDataHelpersSelectorBuilder
{
    public Type SelectorItemType { get; }

    public Type SelectorItemServiceType { get; }

    public DynamicDataHelpersSelectorBuilder(Type selectorItemType, Type selectorItemServiceType)
    {
        SelectorItemType = selectorItemType;
        SelectorItemServiceType = selectorItemServiceType;
    }

    public DynamicDataHelpersSelector Build(IServiceCollection services)
    {
        services.AddScoped(typeof(ISelectorDataProvider<>).MakeGenericType(SelectorItemType), SelectorItemServiceType);

        return new DynamicDataHelpersSelector(SelectorItemType, SelectorItemServiceType);
    }

}