using DotVVM.DynamicData.Helpers.Services;
using DotVVM.DynamicData.Helpers.ViewModels;
using DotVVM.Framework.Controls.DynamicData.Annotations;
using Microsoft.Extensions.DependencyInjection;

namespace DotVVM.DynamicData.Helpers.Configuration.Builders;

public class DynamicDataHelpersDetailPageBuilder<TModel, TKey> : DynamicDataHelpersPageBuilder
{

    private DynamicDataHelpersServiceBuilder? serviceBuilder;

    public DynamicDataHelpersDetailPageBuilder(DynamicDataHelpersSectionBuilder section) : base("Detail", section)
    {
    }

    public new DynamicDataHelpersDetailPageBuilder<TModel, TKey> SetPageName(string pageName)
    {
        base.SetPageName(pageName);
        return this;
    }

    public new DynamicDataHelpersDetailPageBuilder<TModel, TKey> AddSelector<TSelectorItem>()
        where TSelectorItem : SelectorItem
    {
        base.AddSelector<TSelectorItem>();
        return this;
    }

    public new DynamicDataHelpersDetailPageBuilder<TModel, TKey> IncludeInMenu(bool includeInMenu)
    {
        base.IncludeInMenu(includeInMenu);
        return this;
    }

    public DynamicDataHelpersDetailPageBuilder<TModel, TKey> UseService(DynamicDataHelpersServiceBuilder serviceBuilder)
    {
        this.serviceBuilder = serviceBuilder;
        return this;
    }

    protected override string GetViewMarkup() => GetTemplateMarkup("Detail");

    public override bool IsListPage => false;
    
    public override bool IsDetailPage => true;

    public override string GetRouteUrl() => $"{base.GetRouteUrl()}/{{Id?}}";

    protected override Type GetViewModelType()
    {
        var viewModelType = typeof(DetailPageViewModel<TModel, TKey>);
        var selectorsTupleTypes = GetSelectorsTupleType();
        return Section.GlobalConfiguration.ViewModelHostType.MakeGenericType(viewModelType, selectorsTupleTypes);
    }

    protected override void RegisterServices(IServiceCollection services)
    {
        if (serviceBuilder == null)
        {
            throw new Exception($"The detail page in {Section.SectionName} doesn't have any service configured! Please use UseService();");
        }
        serviceBuilder.Build(services);

        services.AddScoped<DetailPageViewModel<TModel, TKey>>();
    }

}