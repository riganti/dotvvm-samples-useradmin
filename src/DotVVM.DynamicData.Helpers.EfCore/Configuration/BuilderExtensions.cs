using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DotVVM.DynamicData.Helpers.Configuration.Builders;
using DotVVM.DynamicData.Helpers.EfCore.Configuration.Builders;
using Microsoft.EntityFrameworkCore;

// ReSharper disable once CheckNamespace
namespace DotVVM.DynamicData.Helpers
{
    public static class BuilderExtensions
    {

        public static DynamicDataHelpersListPageBuilder<TModel> UseEfCoreService<TDbContext, TEntity, TModel>(this DynamicDataHelpersListPageBuilder<TModel> pageBuilder, 
            Action<DynamicDataHelpersEfCoreListServiceBuilder<TDbContext, TEntity, TModel>>? configure = null)
            where TDbContext : DbContext 
            where TModel : new()
        {
            var serviceBuilder = new DynamicDataHelpersEfCoreListServiceBuilder<TDbContext, TEntity, TModel>(pageBuilder);
            configure?.Invoke(serviceBuilder);
            pageBuilder.UseService(serviceBuilder);
            return pageBuilder;
        }

        public static DynamicDataHelpersDetailPageBuilder<TModel, TKey> UseEfCoreService<TDbContext, TEntity, TModel, TKey>(this DynamicDataHelpersDetailPageBuilder<TModel, TKey> pageBuilder,
            Action<DynamicDataHelpersEfCoreDetailServiceBuilder<TDbContext, TEntity, TModel, TKey>>? configure = null)
            where TDbContext : DbContext
            where TModel : new()
            where TEntity : class, new()
        {
            var serviceBuilder = new DynamicDataHelpersEfCoreDetailServiceBuilder<TDbContext, TEntity, TModel, TKey>(pageBuilder);
            configure?.Invoke(serviceBuilder);
            pageBuilder.UseService(serviceBuilder);
            return pageBuilder;
        }

    }
}
