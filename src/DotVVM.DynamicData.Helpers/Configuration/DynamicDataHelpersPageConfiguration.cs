namespace DotVVM.DynamicData.Helpers.Configuration;

public class DynamicDataHelpersPageConfiguration
{
    public string PageName { get; }
    public Type ServiceType { get; }
    public IReadOnlySet<Type> RequiredSelectorTypes { get; }
    public string RouteName { get; }
    public object RouteParameters { get; }
    public Func<string> PageTitleProvider { get; }
    public string ViewMarkup { get; }
    public IReadOnlyList<ToolbarButton> ToolbarButtons { get; }
    public bool IncludeInMenu { get; }

    public DynamicDataHelpersPageConfiguration(string pageName, Type serviceType, IReadOnlySet<Type> requiredSelectorTypes, string routeName, object routeParameters, Func<string> pageTitleProvider, string viewMarkup, IReadOnlyList<ToolbarButton> toolbarButtons, bool includeInMenu)
    {
        PageName = pageName;
        ServiceType = serviceType;
        RequiredSelectorTypes = requiredSelectorTypes;
        RouteName = routeName;
        RouteParameters = routeParameters;
        PageTitleProvider = pageTitleProvider;
        ViewMarkup = viewMarkup;
        ToolbarButtons = toolbarButtons;
        IncludeInMenu = includeInMenu;
    }
}