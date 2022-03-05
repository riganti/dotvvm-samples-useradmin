using DotVVM.DynamicData.Helpers.Configuration;
using DotVVM.DynamicData.Helpers.Configuration.Builders;
using DotVVM.DynamicData.Helpers.Model;
using DotVVM.Framework.Hosting;

namespace DotVVM.DynamicData.Helpers.Metadata;

public class PageMetadataProvider : IPageMetadataProvider
{
    private readonly DynamicDataHelpersConfiguration configuration;

    public PageMetadataProvider(DynamicDataHelpersConfiguration configuration)
    {
        this.configuration = configuration;
    }

    public PageMetadata GetPage(IDotvvmRequestContext context)
    {
        var page = GetPageConfiguration(context);

        return new PageMetadata()
        {
            Title = page.PageTitleProvider(),
            ToolbarButtons = page.ToolbarButtons
                .Select(b => CreateButtonModel(b, context))
                .ToList()
        };
    }

    public DynamicDataHelpersPageConfiguration GetPageConfiguration(IDotvvmRequestContext context)
    {
        var allPages = configuration.Sections
            .SelectMany(s => s.Pages.Select(p => new { Section = s, Page = p }))
            .ToList();

        var page = allPages.SingleOrDefault(p => p.Page.RouteName == context.Route.RouteName);
        if (page == null)
        {
            throw new Exception($"Cannot obtain Dynamic Data Helpers page metadata for route {context.Route.RouteName}.");
        }

        return page.Page;
    }

    private ToolbarButtonModel CreateButtonModel(ToolbarButton button, IDotvvmRequestContext context)
    {
        var result = new ToolbarButtonModel()
        {
            Text = button.TextProvider()
        };

        if (!string.IsNullOrEmpty(button.Url))
        {
            result.Url = button.Url;
        }
        else
        {
            result.Url = context.TranslateVirtualPath(context.Configuration.RouteTable[button.RouteName].BuildUrl(button.RouteParameters));
        }

        return result;
    }
}