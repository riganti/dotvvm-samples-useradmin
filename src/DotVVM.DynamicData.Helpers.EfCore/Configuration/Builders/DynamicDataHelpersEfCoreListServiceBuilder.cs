using DotVVM.DynamicData.Helpers.Configuration;
using DotVVM.DynamicData.Helpers.Configuration.Builders;
using DotVVM.DynamicData.Helpers.EfCore.Services;
using DotVVM.DynamicData.Helpers.Services;
using DotVVM.Framework.Controls;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace DotVVM.DynamicData.Helpers.EfCore.Configuration.Builders;

public class DynamicDataHelpersEfCoreListServiceBuilder<TDbContext, TEntity, TModel> : DynamicDataHelpersServiceBuilder 
    where TModel : new() 
    where TDbContext : DbContext
{
    private Func<IQueryable<TEntity>, IQueryable<TEntity>>? entityFilter = null;
    private Func<IQueryable<TEntity>, IQueryable<TModel>>? projection = null;
    private Func<IGridViewDataSet<TModel>, TDbContext, Task>? postProcessor = null;

    public DynamicDataHelpersListPageBuilder<TModel> Page { get; }

    public DynamicDataHelpersEfCoreListServiceBuilder(DynamicDataHelpersListPageBuilder<TModel> page)
    {
        Page = page;
    }

    public DynamicDataHelpersEfCoreListServiceBuilder<TDbContext, TEntity, TModel> UseEntityFilter(
        Func<IQueryable<TEntity>, IQueryable<TEntity>> entityFilter)
    {
        this.entityFilter = entityFilter;
        return this;
    }

    public DynamicDataHelpersEfCoreListServiceBuilder<TDbContext, TEntity, TModel> UseProjection(
        Func<IQueryable<TEntity>, IQueryable<TModel>> projection)
    {
        this.projection = projection;
        return this;
    }

    public DynamicDataHelpersEfCoreListServiceBuilder<TDbContext, TEntity, TModel> UsePostProcessor(
        Func<IGridViewDataSet<TModel>, TDbContext, Task> postProcessor)
    {
        this.postProcessor = postProcessor;
        return this;
    }

    public override void Build(IServiceCollection serviceCollection)
    {
        var interfaceType = typeof(IListPageService<,>).MakeGenericType(typeof(TModel), Page.FilterModelType);
        var serviceType = typeof(EfCoreListPageService<,,,>)
            .MakeGenericType(typeof(TDbContext), typeof(TEntity), typeof(TModel), Page.FilterModelType);

        serviceCollection.AddScoped(interfaceType, 
            provider => Activator.CreateInstance(serviceType, 
                new object?[]
                {
                    provider.GetRequiredService<TDbContext>(),
                    entityFilter,
                    projection,
                    postProcessor
                })!);
    }
 }