using System.Resources;

namespace DotVVM.DynamicData.Helpers.Configuration;

public class DynamicDataHelpersConfiguration
{
    public IReadOnlyList<DynamicDataHelpersSection> Sections { get; }

    public IReadOnlyList<DynamicDataHelpersSelector> Selectors { get; }

    public ResourceManager? ResourceManager { get; }

    public string MasterPagePath { get; }

    public Type ViewModelHostType { get; }

    public DynamicDataHelpersConfiguration(string masterPagePath, Type viewModelHostType, IReadOnlyList<DynamicDataHelpersSection> sections, IReadOnlyList<DynamicDataHelpersSelector> selectors, ResourceManager? resourceManager)
    {
        Sections = sections;
        Selectors = selectors;
        MasterPagePath = masterPagePath;
        ViewModelHostType = viewModelHostType;
        ResourceManager = resourceManager;
    }

    public DynamicDataHelpersSection GetSection(string sectionName)
    {
        var section = Sections.SingleOrDefault(s => s.SectionName == sectionName);
        if (section == null)
        {
            throw new Exception($"Section {sectionName} was not found!");
        }
        return section;
    }
}