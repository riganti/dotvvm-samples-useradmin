using DotVVM.DynamicData.Helpers.Configuration;
using DotVVM.DynamicData.Helpers.Context;
using DotVVM.Framework.Hosting;

namespace DotVVM.DynamicData.Helpers.EfCore.Context;

public class EfCoreServiceContext<TDbContext> : ServiceContext
{
    public TDbContext DbContext { get; }

    public EfCoreServiceContext(IDotvvmRequestContext dotvvmRequestContext, DynamicDataHelpersPageConfiguration pageConfiguration, TDbContext dbContext) : base(dotvvmRequestContext, pageConfiguration)
    {
        DbContext = dbContext;
    }
}