using DotVVM.DynamicData.Helpers.Configuration;
using DotVVM.DynamicData.Helpers.Model;

namespace DotVVM.DynamicData.Helpers.Metadata;

public class PageMetadata
{
    public string Title { get; set; }

    public List<ToolbarButtonModel> ToolbarButtons { get; set; }
}