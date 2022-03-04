using DotVVM.DynamicData.Helpers.Model;
using DotVVM.DynamicData.Helpers.Services;
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

    public DynamicDataHelpersSectionBuilder AddListPage<TModel>(Action<DynamicDataHelpersListPageBuilder<TModel>>? configure = null)
    {
        return AddListPage<TModel, EmptyFilterModel>();
    }

    public DynamicDataHelpersSectionBuilder AddListPage<TModel, TFilter>(Action<DynamicDataHelpersListPageBuilder<TModel>>? configure = null)
    {
        var page = new DynamicDataHelpersListPageBuilder<TModel>(typeof(TFilter), this);
        configure?.Invoke(page);

        pages.Add(page);

        return this;
    }

    public DynamicDataHelpersSectionBuilder AddListPage<TModel, TFilter, TService>(Action<DynamicDataHelpersListPageBuilder<TModel>>? configure = null) 
        where TService : IListPageService<TModel, TFilter>
    {
        return AddListPage<TModel, TFilter>(page =>
        {
            configure?.Invoke(page);
            page.UseService(new DynamicDataHelpersListPageServiceBuilder<TModel, TFilter, TService>());
        });
    }

    public DynamicDataHelpersSectionBuilder AddDetailPage<TModel, TKey>(Action<DynamicDataHelpersDetailPageBuilder<TModel, TKey>>? configure = null)
    {
        var page = new DynamicDataHelpersDetailPageBuilder<TModel, TKey>(this);
        configure?.Invoke(page);

        pages.Add(page);

        return this;
    }

    public DynamicDataHelpersSectionBuilder AddDetailPage<TModel, TKey, TService>(Action<DynamicDataHelpersDetailPageBuilder<TModel, TKey>>? configure = null)
        where TService : IDetailPageService<TModel, TKey>
    {
        return AddDetailPage<TModel, TKey>(page =>
        {
            configure?.Invoke(page);
            page.UseService(new DynamicDataHelpersDetailPageServiceBuilder<TModel, TKey, TService>());
        });
    }


    public DynamicDataHelpersSection Build(IServiceCollection services)
    {
        return new DynamicDataHelpersSection(
            SectionName,
            pages.Select(p => p.Build(services)).ToList()
        );
    }

}