using DotVVM.DynamicData.Helpers.Configuration;
using DotVVM.Framework.Hosting;

namespace DotVVM.DynamicData.Helpers.Metadata;

public interface IPageMetadataProvider
{

    PageMetadata GetPage(IDotvvmRequestContext context);

    DynamicDataHelpersPageConfiguration GetPageConfiguration(IDotvvmRequestContext context);
}