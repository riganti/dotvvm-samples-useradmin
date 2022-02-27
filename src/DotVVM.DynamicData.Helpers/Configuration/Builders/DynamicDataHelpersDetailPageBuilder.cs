using DotVVM.DynamicData.Helpers.Services;
using DotVVM.DynamicData.Helpers.ViewModels;
using DotVVM.Framework.Controls.DynamicData.Annotations;
using Microsoft.Extensions.DependencyInjection;

namespace DotVVM.DynamicData.Helpers.Configuration.Builders;

public class DynamicDataHelpersDetailPageBuilder : DynamicDataHelpersPageBuilder
{
    private Type interfaceType;

    public DynamicDataHelpersDetailPageBuilder(Type serviceType, DynamicDataHelpersSectionBuilder section) : base("Detail", serviceType, section)
    {
        interfaceType = EnsureServiceType(serviceType, typeof(IDetailPageService<,>));
    }

    public new DynamicDataHelpersDetailPageBuilder SetPageName(string pageName)
    {
        base.SetPageName(pageName);
        return this;
    }

    public new DynamicDataHelpersDetailPageBuilder AddSelector<TSelectorItem>()
        where TSelectorItem : SelectorItem
    {
        base.AddSelector<TSelectorItem>();
        return this;
    }

    public new DynamicDataHelpersDetailPageBuilder IncludeInMenu(bool includeInMenu)
    {
        base.IncludeInMenu(includeInMenu);
        return this;
    }

    protected override string GetViewMarkup() => GetTemplateMarkup("Detail");

    public override string GetRouteUrl() => $"{base.GetRouteUrl()}/{{Id?}}";

    protected override Type GetViewModelType()
    {
        var viewModelType = typeof(DetailPageViewModel<,>).MakeGenericType(interfaceType.GetGenericArguments());
        var selectorsTupleTypes = GetSelectorsTupleType();
        return Section.GlobalConfiguration.ViewModelHostType.MakeGenericType(viewModelType, selectorsTupleTypes);
    }

    protected override void RegisterServices(IServiceCollection services)
    {
        services.AddScoped(interfaceType, ServiceType);
        services.AddScoped(typeof(DetailPageViewModel<,>).MakeGenericType(interfaceType.GetGenericArguments()));
    }
}