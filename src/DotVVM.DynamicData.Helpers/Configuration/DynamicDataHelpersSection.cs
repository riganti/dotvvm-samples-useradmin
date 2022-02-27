namespace DotVVM.DynamicData.Helpers.Configuration;

public class DynamicDataHelpersSection
{
    public string SectionName { get; }

    public List<DynamicDataHelpersPageConfiguration> Pages { get; }

    public DynamicDataHelpersSection(string sectionName, List<DynamicDataHelpersPageConfiguration> pages)
    {
        SectionName = sectionName;
        Pages = pages;
    }

    public DynamicDataHelpersPageConfiguration GetPage(string pageName)
    {
        var page = Pages.SingleOrDefault(p => p.PageName == pageName);
        if (page == null)
        {
            throw new Exception($"Page {pageName} was not found!");
        }
        return page;
    }
}