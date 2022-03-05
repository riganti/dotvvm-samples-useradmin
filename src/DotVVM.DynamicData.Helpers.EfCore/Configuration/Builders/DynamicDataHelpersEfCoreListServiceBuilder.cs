using System.Reflection;
using DotVVM.DynamicData.Helpers.Configuration;
using DotVVM.DynamicData.Helpers.Configuration.Builders;
using DotVVM.DynamicData.Helpers.Context;
using DotVVM.DynamicData.Helpers.EfCore.Context;
using DotVVM.DynamicData.Helpers.EfCore.Services;
using DotVVM.DynamicData.Helpers.Services;
using DotVVM.Framework.Controls;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace DotVVM.DynamicData.Helpers.EfCore.Configuration.Builders;

public class DynamicDataHelpersEfCoreListServiceBuilder<TDbContext, TEntity, TModel, TFilterModel> : DynamicDataHelpersServiceBuilder, IListServiceBuilderWithMapping<TEntity, TModel> 
    where TModel : new() 
    where TDbContext : DbContext
    where TEntity : class
{
    private Func<IQueryable<TEntity>, TFilterModel, EfCoreServiceContext<TDbContext>, IQueryable<TEntity>>? entityFilter = null;
    private Func<IQueryable<TEntity>, EfCoreServiceContext<TDbContext>, IQueryable<TModel>>? projection = null;
    private Func<IGridViewDataSet<TModel>, EfCoreServiceContext<TDbContext>, Task>? postProcessor = null;

    public DynamicDataHelpersListPageBuilder<TModel> Page { get; }

    public DynamicDataHelpersEfCoreListServiceBuilder(DynamicDataHelpersListPageBuilder<TModel> page)
    {
        Page = page;
    }

    public DynamicDataHelpersEfCoreListServiceBuilder<TDbContext, TEntity, TModel, TFilterModel> UseEntityFilter(
        Func<IQueryable<TEntity>, IQueryable<TEntity>> entityFilter)
    {
        this.entityFilter = (entities, filter, context) => entityFilter(entities);
        return this;
    }

    public DynamicDataHelpersEfCoreListServiceBuilder<TDbContext, TEntity, TModel, TFilterModel> UseEntityFilter(
        Func<IQueryable<TEntity>, TFilterModel, IQueryable<TEntity>> entityFilter)
    {
        this.entityFilter = (entities, filter, context) => entityFilter(entities, filter);
        return this;
    }

    public DynamicDataHelpersEfCoreListServiceBuilder<TDbContext, TEntity, TModel, TFilterModel> UseEntityFilter(
        Func<IQueryable<TEntity>, EfCoreServiceContext<TDbContext>, IQueryable<TEntity>> entityFilter)
    {
        this.entityFilter = (entities, filter, context) => entityFilter(entities, context);
        return this;
    }

    public DynamicDataHelpersEfCoreListServiceBuilder<TDbContext, TEntity, TModel, TFilterModel> UseEntityFilter(
        Func<IQueryable<TEntity>, TFilterModel, EfCoreServiceContext<TDbContext>, IQueryable<TEntity>> entityFilter)
    {
        this.entityFilter = entityFilter;
        return this;
    }

    public DynamicDataHelpersEfCoreListServiceBuilder<TDbContext, TEntity, TModel, TFilterModel> UseMapping(
        Func<IQueryable<TEntity>, IQueryable<TModel>> projection)
    {
        this.projection = (entities, context) => projection(entities);
        return this;
    }

    public DynamicDataHelpersEfCoreListServiceBuilder<TDbContext, TEntity, TModel, TFilterModel> UseMapping(
        Func<IQueryable<TEntity>, EfCoreServiceContext<TDbContext>, IQueryable<TModel>> projection)
    {
        this.projection = projection;
        return this;
    }

    IListServiceBuilderWithMapping<TEntity, TModel> IListServiceBuilderWithMapping<TEntity, TModel>.UseMapping(Func<IQueryable<TEntity>, ServiceContext, IQueryable<TModel>> projection)
    {
        return UseMapping(projection);
    }

    public DynamicDataHelpersEfCoreListServiceBuilder<TDbContext, TEntity, TModel, TFilterModel> UsePostProcessor(
        Func<IGridViewDataSet<TModel>, EfCoreServiceContext<TDbContext>, Task> postProcessor)
    {
        this.postProcessor = postProcessor;
        return this;
    }

    public override void Build(IServiceCollection serviceCollection)
    {
        serviceCollection.AddScoped<IListPageService<TModel, TFilterModel>>(
            provider => new EfCoreListPageService<TDbContext, TEntity, TModel, TFilterModel>(
                provider.GetRequiredService<TDbContext>(),
                provider.GetRequiredService<ServiceContext>(),
                entityFilter,
                projection,
                postProcessor
            ));
    }
}