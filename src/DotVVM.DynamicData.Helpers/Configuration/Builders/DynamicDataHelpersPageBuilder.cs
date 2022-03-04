using System.Text;
using DotVVM.DynamicData.Helpers.Model;
using DotVVM.DynamicData.Helpers.Services;
using DotVVM.Framework.Configuration;
using DotVVM.Framework.Controls.DynamicData.Annotations;
using DotVVM.Framework.Controls.DynamicData.ViewModel;
using DotVVM.Framework.Utils;
using Microsoft.Extensions.DependencyInjection;

namespace DotVVM.DynamicData.Helpers.Configuration.Builders;

public abstract class DynamicDataHelpersPageBuilder
{
    private readonly HashSet<Type> requiredSelectorTypes = new();

    public string PageName { get; protected set; }

    public IReadOnlySet<Type> RequiredSelectorTypes => requiredSelectorTypes;

    public DynamicDataHelpersSectionBuilder Section { get; }

    public bool IncludePageInMenu { get; protected set; }
    
    public abstract bool IsListPage { get; }
    
    public abstract bool IsDetailPage { get; }


    protected DynamicDataHelpersPageBuilder(string pageName, DynamicDataHelpersSectionBuilder section)
    {
        PageName = pageName;
        Section = section;
    }

    public virtual string GetRouteName() => $"{Section.SectionName}_{PageName}";

    public virtual string GetRouteUrl() => $"{Section.SectionName}/{PageName}";

    public virtual object GetRouteParameters() => new { };

    protected virtual string GetTitle()
    {
        return Section.GlobalConfiguration.ResourceManager?.GetString($"Page_{Section.SectionName}_{PageName}_Title")
               ?? $"{Section.SectionName} {PageName}";
    }

    protected abstract string GetViewMarkup();

    protected abstract Type GetViewModelType();

    protected string GetTemplateMarkup(string templateName)
    {
        var stream = typeof(IListPageService<,>).Assembly.GetManifestResourceStream($"DotVVM.DynamicData.Helpers.Templates.{templateName}.dothtml")!;
        using var streamReader = new StreamReader(stream, Encoding.UTF8);
        var markup = streamReader.ReadToEnd();

        return markup
            .Replace("%ViewModelType%", GetViewModelType().ToFullNameString())
            .Replace("%MasterPagePath%", Section.GlobalConfiguration.MasterPagePath);
    }

    protected virtual IEnumerable<ToolbarButton> GetToolbarButtons()
    {
        return Enumerable.Empty<ToolbarButton>();
    }

    protected void SetPageName(string pageName)
    {
        PageName = pageName;
    }

    protected void AddSelector<TSelectorItem>()
        where TSelectorItem : SelectorItem
    {
        requiredSelectorTypes.Add(typeof(TSelectorItem));
    }
    protected void IncludeInMenu(bool includeInMenu)
    {
        IncludePageInMenu = includeInMenu;
    }

    protected Type GetSelectorsTupleType()
    {
        Type selectorsTupleTypes = typeof(EmptyFilterModel);
        if (RequiredSelectorTypes.Count > 0)
        {
            var tupleType = Type.GetType("System.Tuple`" + RequiredSelectorTypes.Count, true)!;
            selectorsTupleTypes = tupleType.MakeGenericType(RequiredSelectorTypes.Select(s => typeof(SelectorViewModel<>).MakeGenericType(s)).ToArray());
        }

        return selectorsTupleTypes;
    }

    public DynamicDataHelpersPageConfiguration Build(IServiceCollection services)
    {
        services.Configure<DotvvmConfiguration>(config =>
        {
            config.RouteTable.Add(GetRouteName(), GetRouteUrl(), $"dynamicdata://{Section.SectionName}/{PageName}");
        });
        
        RegisterServices(services);

        return BuildPageConfiguration();
    }

    protected virtual DynamicDataHelpersPageConfiguration BuildPageConfiguration()
    {
        return new DynamicDataHelpersPageConfiguration(
            PageName, 
            RequiredSelectorTypes, 
            GetRouteName(), 
            GetRouteParameters(), 
            GetTitle, 
            GetViewMarkup(), 
            GetToolbarButtons().ToList(),
            IncludePageInMenu
        );
    }

    protected abstract void RegisterServices(IServiceCollection services);
}