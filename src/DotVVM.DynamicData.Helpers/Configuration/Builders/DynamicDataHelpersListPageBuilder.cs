using DotVVM.DynamicData.Helpers.Services;
using DotVVM.DynamicData.Helpers.ViewModels;
using DotVVM.Framework.Controls.DynamicData.Annotations;
using Microsoft.Extensions.DependencyInjection;

namespace DotVVM.DynamicData.Helpers.Configuration.Builders;

public class DynamicDataHelpersListPageBuilder : DynamicDataHelpersPageBuilder
{
    private readonly Type interfaceType;

    public DynamicDataHelpersListPageBuilder(Type serviceType, DynamicDataHelpersSectionBuilder section) : base("List", serviceType, section)
    {
        interfaceType = EnsureServiceType(serviceType, typeof(IListPageService<,>));

        IncludePageInMenu = true;
    }

    public new DynamicDataHelpersListPageBuilder SetPageName(string pageName)
    {
        base.SetPageName(pageName);
        return this;
    }

    public new DynamicDataHelpersListPageBuilder AddSelector<TSelectorItem>()
        where TSelectorItem : SelectorItem
    {
        base.AddSelector<TSelectorItem>();
        return this;
    }

    public new DynamicDataHelpersListPageBuilder IncludeInMenu(bool includeInMenu)
    {
        base.IncludeInMenu(includeInMenu);
        return this;
    }

    protected override string GetViewMarkup() => GetTemplateMarkup("List");

    protected override Type GetViewModelType()
    {
        var viewModelType = typeof(ListPageViewModel<,>).MakeGenericType(interfaceType.GetGenericArguments());
        var selectorsTupleTypes = GetSelectorsTupleType();
        return Section.GlobalConfiguration.ViewModelHostType.MakeGenericType(viewModelType, selectorsTupleTypes);
    }

    protected override IEnumerable<ToolbarButton> GetToolbarButtons()
    {
        var detailPage = Section.Pages.OfType<DynamicDataHelpersDetailPageBuilder>().FirstOrDefault();
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
        services.AddScoped(interfaceType, ServiceType);
        services.AddScoped(typeof(ListPageViewModel<,>).MakeGenericType(interfaceType.GetGenericArguments()));
    }
}