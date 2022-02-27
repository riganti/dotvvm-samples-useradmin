using System.ComponentModel.DataAnnotations;
using System.Reflection;
using DotVVM.DynamicData.Helpers.Metadata;
using DotVVM.DynamicData.Helpers.Model;
using DotVVM.Framework.ViewModel;
using Microsoft.Extensions.DependencyInjection;

namespace DotVVM.DynamicData.Helpers.ViewModels;

public class DynamicPageViewModelBase : DotvvmViewModelBase
{

    [Bind(Direction.ServerToClientFirstRequest)]
    public string Title { get; set; }

    [Bind(Direction.ServerToClientFirstRequest)]
    public List<ToolbarButtonModel> ToolbarButtons { get; set; }


    public override Task Init()
    {
        if (!Context.IsPostBack)
        {
            LoadPageMetadata();
        }

        return base.Init();
    }

    protected void LoadPageMetadata()
    {
        var pageMetadata = Context.Services.GetRequiredService<IPageMetadataProvider>().GetPage(Context);
            
        Title = pageMetadata.Title;
        ToolbarButtons = pageMetadata.ToolbarButtons;
    }

    public static TSelectorsTuple InitSelectors<TSelectorsTuple>()
    {
        var args = typeof(TSelectorsTuple).GetGenericArguments().Select(Activator.CreateInstance).ToArray();
        return (TSelectorsTuple)Activator.CreateInstance(typeof(TSelectorsTuple), args)!;
    }

    public static IEnumerable<IDotvvmViewModel> GetSelectorViewModels<TSelectorsTuple>(TSelectorsTuple selectors)
    {
        return typeof(TSelectorsTuple)
            .GetProperties(BindingFlags.Public | BindingFlags.Instance)
            .Where(p => typeof(IDotvvmViewModel).IsAssignableFrom(p.PropertyType))
            .Select(p => p.GetValue(selectors))
            .OfType<IDotvvmViewModel>();
    }
}