using System.Resources;
using DotVVM.Framework.Controls.DynamicData.Annotations;
using Microsoft.Extensions.DependencyInjection;

namespace DotVVM.DynamicData.Helpers.Configuration.Builders;

public class DynamicDataHelpersConfigurationBuilder
{

    private List<DynamicDataHelpersSectionBuilder> sections = new();
    private List<DynamicDataHelpersSelectorBuilder> selectors = new();

    public IReadOnlyList<DynamicDataHelpersSectionBuilder> Sections => sections;

    public ResourceManager? ResourceManager { get; private set; }

    public string MasterPagePath { get; }

    public Type ViewModelHostType { get; }

    public DynamicDataHelpersConfigurationBuilder(string masterPagePath, Type viewModelHostType)
    {
        MasterPagePath = masterPagePath;

        // TODO: validate
        ViewModelHostType = viewModelHostType;
    }

    public DynamicDataHelpersConfigurationBuilder AddSection(string name, Action<DynamicDataHelpersSectionBuilder> configure)
    {
        // TODO: check name validity
        if (sections.Any(s => string.Equals(s.SectionName, name, StringComparison.OrdinalIgnoreCase)))
        {
            throw new ArgumentException("Section with this name already exists!", nameof(name));
        }

        var builder = new DynamicDataHelpersSectionBuilder(name, this);
        configure(builder);
        sections.Add(builder);

        return this;
    }

    public DynamicDataHelpersConfigurationBuilder AddSelector<TSelectorItem, TSelectorItemService>()
        where TSelectorItem : SelectorItem
        where TSelectorItemService : class, ISelectorDataProvider<TSelectorItem>
    {
        selectors.Add(new DynamicDataHelpersSelectorBuilder(typeof(TSelectorItem), typeof(TSelectorItemService)));
        return this;
    }

    public DynamicDataHelpersConfigurationBuilder UseResourceFile(Type resourceType)
    {
        ResourceManager = new ResourceManager(resourceType);
        return this;
    }


    public DynamicDataHelpersConfiguration Build(IServiceCollection services)
    {
        return new DynamicDataHelpersConfiguration(MasterPagePath,
            ViewModelHostType,
            sections: sections.Select(s => s.Build(services)).ToList(), selectors: selectors.Select(s => s.Build(services)).ToList(), resourceManager: ResourceManager);
    }

    
}