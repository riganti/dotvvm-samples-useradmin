using System.Linq.Expressions;
using DotVVM.DynamicData.Helpers.Configuration.Builders;
using DotVVM.DynamicData.Helpers.EfCore.Services;
using DotVVM.DynamicData.Helpers.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace DotVVM.DynamicData.Helpers.EfCore.Configuration.Builders;

public class DynamicDataHelpersEfCoreDetailServiceBuilder<TDbContext, TEntity, TModel, TKey> : DynamicDataHelpersServiceBuilder
    where TModel : new()
    where TDbContext : DbContext
    where TEntity : class, new()
{
    public DynamicDataHelpersDetailPageBuilder<TModel, TKey> Page { get; }

    private Func<TEntity, TModel>? map;
    private Action<TEntity, TModel>? mapBack;
    private Func<IQueryable<TEntity>, IQueryable<TEntity>>? entityFilter;

    public DynamicDataHelpersEfCoreDetailServiceBuilder(DynamicDataHelpersDetailPageBuilder<TModel, TKey> page)
    {
        Page = page;
    }

    public DynamicDataHelpersEfCoreDetailServiceBuilder<TDbContext, TEntity, TModel, TKey> UseEntityFilter(
        Func<IQueryable<TEntity>, IQueryable<TEntity>> entityFilter)
    {
        this.entityFilter = entityFilter;
        return this;
    }

    public DynamicDataHelpersEfCoreDetailServiceBuilder<TDbContext, TEntity, TModel, TKey> UseMapping(
        Func<TEntity, TModel> map, Action<TEntity, TModel> mapBack)
    {
        this.map = map;
        this.mapBack = mapBack;
        return this;
    }

    public override void Build(IServiceCollection services)
    {
        var interfaceType = typeof(IDetailPageService<TModel, TKey>);
        var serviceType = typeof(EfCoreDetailPageService<TDbContext, TEntity, TModel, TKey>);

        services.AddScoped(interfaceType,
            provider => Activator.CreateInstance(serviceType,
                new object?[]
                {
                    provider.GetRequiredService<TDbContext>(),
                    entityFilter,
                    map,
                    mapBack
                })!);
    }
}