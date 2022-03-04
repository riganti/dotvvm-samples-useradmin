using DotVVM.DynamicData.Helpers.Model;
using DotVVM.DynamicData.Helpers.Services;
using DotVVM.DynamicData.Helpers.ViewModels;
using DotVVM.Framework.Controls.DynamicData.Annotations;
using Microsoft.Extensions.DependencyInjection;

namespace DotVVM.DynamicData.Helpers.Configuration.Builders;

public class DynamicDataHelpersListPageBuilder<TModel> : DynamicDataHelpersPageBuilder
{
    private DynamicDataHelpersServiceBuilder? serviceBuilder;

    public Type FilterModelType { get; }

    public DynamicDataHelpersListPageBuilder(Type filterModelType, DynamicDataHelpersSectionBuilder section) : base("List", section)
    {
        IncludePageInMenu = true;
        FilterModelType = filterModelType;
    }

    public new DynamicDataHelpersListPageBuilder<TModel> SetPageName(string pageName)
    {
        base.SetPageName(pageName);
        return this;
    }

    public new DynamicDataHelpersListPageBuilder<TModel> AddSelector<TSelectorItem>()
        where TSelectorItem : SelectorItem
    {
        base.AddSelector<TSelectorItem>();
        return this;
    }

    public new DynamicDataHelpersListPageBuilder<TModel> IncludeInMenu(bool includeInMenu)
    {
        base.IncludeInMenu(includeInMenu);
        return this;
    }

    public DynamicDataHelpersListPageBuilder<TModel> UseService(DynamicDataHelpersServiceBuilder serviceBuilder)
    {
        this.serviceBuilder = serviceBuilder;
        return this;
    }

    public override bool IsListPage => true;
    
    public override bool IsDetailPage => false;

    protected override string GetViewMarkup() => GetTemplateMarkup("List");

    protected override Type GetViewModelType()
    {
        var viewModelType = typeof(ListPageViewModel<,>).MakeGenericType(typeof(TModel), FilterModelType);
        var selectorsTupleTypes = GetSelectorsTupleType();
        return Section.GlobalConfiguration.ViewModelHostType.MakeGenericType(viewModelType, selectorsTupleTypes);
    }

    protected override IEnumerable<ToolbarButton> GetToolbarButtons()
    {
        var detailPage = Section.Pages.FirstOrDefault(p => p.IsDetailPage);
        if (detailPage != null)
        {
            yield return new ToolbarButton()
            {
                TextProvider = GetInsertButtonText,
                RouteName = detailPage.GetRouteName(),
                RouteParameters = new { }
            };
        }

        foreach (var button in base.GetToolbarButtons())
        {
            yield return button;
        }
    }

    private string GetInsertButtonText()
    {
        return Section.GlobalConfiguration.ResourceManager?.GetString($"Page_{Section.SectionName}_{PageName}_InsertButtonText")
               ?? Section.GlobalConfiguration.ResourceManager?.GetString($"Global_InsertButtonText")
               ?? "Create new";
    }

    protected override void RegisterServices(IServiceCollection services)
    {
        if (serviceBuilder == null)
        {
            throw new Exception($"The list page in {Section.SectionName} doesn't have any service configured! Please use UseService();");
        }
        serviceBuilder.Build(services);

        services.AddScoped(typeof(ListPageViewModel<,>).MakeGenericType(typeof(TModel), FilterModelType));
    }
}