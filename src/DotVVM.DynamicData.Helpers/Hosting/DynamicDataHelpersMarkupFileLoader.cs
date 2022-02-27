using DotVVM.DynamicData.Helpers.Configuration;
using DotVVM.DynamicData.Helpers.Configuration.Builders;
using DotVVM.Framework.Configuration;
using DotVVM.Framework.Hosting;

namespace DotVVM.DynamicData.Helpers.Hosting;

public class DynamicDataHelpersMarkupFileLoader : IMarkupFileLoader
{
    private readonly DynamicDataHelpersConfiguration options;

    public DynamicDataHelpersMarkupFileLoader(DynamicDataHelpersConfiguration options)
    {
        this.options = options;
    }

    public string GetMarkupFileVirtualPath(IDotvvmRequestContext context)
    {
        return context.Route.VirtualPath;
    }

    public MarkupFile? GetMarkup(DotvvmConfiguration configuration, string virtualPath)
    {
        if (!virtualPath.StartsWith("dynamicdata://", StringComparison.Ordinal))
        {
            return null;
        }

        var path = virtualPath.Remove(0, "dynamicdata://".Length);
        var parts = path.Split('/');
        if (parts.Length != 2)
        {
            throw new Exception("Invalid format of dynamicdata:// URI - it must have exactly two parts!");
        }

        var sectionName = parts[0];
        var pageName = parts[1];

        var page = options.GetSection(sectionName).GetPage(pageName);

        return new MarkupFile($"{pageName}.dothtml", $"dynamicdata://{sectionName}/{pageName}", page.ViewMarkup);
    }
}