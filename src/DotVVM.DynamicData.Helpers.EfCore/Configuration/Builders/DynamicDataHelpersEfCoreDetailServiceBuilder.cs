using DotVVM.DynamicData.Helpers.Configuration.Builders;
using DotVVM.DynamicData.Helpers.Context;
using DotVVM.DynamicData.Helpers.EfCore.Context;
using DotVVM.DynamicData.Helpers.EfCore.Services;
using DotVVM.DynamicData.Helpers.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace DotVVM.DynamicData.Helpers.EfCore.Configuration.Builders;

public class DynamicDataHelpersEfCoreDetailServiceBuilder<TDbContext, TEntity, TModel, TKey> : DynamicDataHelpersServiceBuilder, IDetailServiceBuilderWithMapping<TEntity, TModel>
    where TModel : new()
    where TDbContext : DbContext
    where TEntity : class, new()
{
    public DynamicDataHelpersDetailPageBuilder<TModel, TKey> Page { get; }

    private Func<TEntity, EfCoreServiceContext<TDbContext>, TModel>? map;
    private Action<TEntity, TModel, EfCoreServiceContext<TDbContext>>? mapBack;
    private Func<IQueryable<TEntity>, EfCoreServiceContext<TDbContext>, IQueryable<TEntity>>? entityFilter;

    public DynamicDataHelpersEfCoreDetailServiceBuilder(DynamicDataHelpersDetailPageBuilder<TModel, TKey> page)
    {
        Page = page;
    }

    public DynamicDataHelpersEfCoreDetailServiceBuilder<TDbContext, TEntity, TModel, TKey> UseEntityFilter(
        Func<IQueryable<TEntity>, IQueryable<TEntity>> entityFilter)
    {
        this.entityFilter = (entities, context) => entityFilter(entities);
        return this;
    }

    public DynamicDataHelpersEfCoreDetailServiceBuilder<TDbContext, TEntity, TModel, TKey> UseEntityFilter(
        Func<IQueryable<TEntity>, EfCoreServiceContext<TDbContext>, IQueryable<TEntity>> entityFilter)
    {
        this.entityFilter = entityFilter;
        return this;
    }

    public DynamicDataHelpersEfCoreDetailServiceBuilder<TDbContext, TEntity, TModel, TKey> UseMapping(
        Func<TEntity, TModel> map, Action<TEntity, TModel> mapBack)
    {
        this.map = (source, context) => map(source);
        this.mapBack = (source, target, context) => mapBack(source, target);
        return this;
    }

    public DynamicDataHelpersEfCoreDetailServiceBuilder<TDbContext, TEntity, TModel, TKey> UseMapping(
        Func<TEntity, EfCoreServiceContext<TDbContext>, TModel> map, Action<TEntity, TModel, EfCoreServiceContext<TDbContext>> mapBack)
    {
        this.map = map;
        this.mapBack = mapBack;
        return this;
    }

    IDetailServiceBuilderWithMapping<TEntity, TModel> IDetailServiceBuilderWithMapping<TEntity, TModel>.UseMapping(Func<TEntity, ServiceContext, TModel> map, Action<TEntity, TModel, ServiceContext> mapBack)
    {
        return UseMapping(map, mapBack);
    }

    public override void Build(IServiceCollection services)
    {
        services.AddScoped<IDetailPageService<TModel, TKey>>(
            provider => new EfCoreDetailPageService<TDbContext, TEntity, TModel, TKey>(
                provider.GetRequiredService<TDbContext>(),
                provider.GetRequiredService<ServiceContext>(),
                entityFilter,
                map,
                mapBack
            ));
    }
}