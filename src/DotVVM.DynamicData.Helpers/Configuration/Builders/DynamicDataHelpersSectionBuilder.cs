using DotVVM.DynamicData.Helpers.Services;
using DotVVM.Framework.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DotVVM.DynamicData.Helpers.Configuration.Builders;

public class DynamicDataHelpersSectionBuilder
{

    private List<DynamicDataHelpersPageBuilder> pages = new();

    public string SectionName { get; }

    public DynamicDataHelpersConfigurationBuilder GlobalConfiguration { get; }

    public IReadOnlyList<DynamicDataHelpersPageBuilder> Pages => pages;

    public DynamicDataHelpersSectionBuilder(string sectionName, DynamicDataHelpersConfigurationBuilder globalConfiguration)
    {
        SectionName = sectionName;
        GlobalConfiguration = globalConfiguration;
    }
    
    public DynamicDataHelpersSectionBuilder AddListPage<TModel, TFilter, TService>(Action<DynamicDataHelpersListPageBuilder>? configure = null)
        where TService : IListPageService<TModel, TFilter>
    {
        var page = new DynamicDataHelpersListPageBuilder(typeof(TService), this);
        configure?.Invoke(page);

        pages.Add(page);

        return this;
    }

    public DynamicDataHelpersSectionBuilder AddDetailPage<TModel, TKey, TService>(Action<DynamicDataHelpersDetailPageBuilder>? configure = null)
        where TService : IDetailPageService<TModel, TKey>
    {
        var page = new DynamicDataHelpersDetailPageBuilder(typeof(TService), this);
        configure?.Invoke(page);

        pages.Add(page);

        return this;
    }


    public DynamicDataHelpersSection Build(IServiceCollection services)
    {
        return new DynamicDataHelpersSection(
            SectionName,
            pages.Select(p => p.Build(services)).ToList()
        );
    }

}