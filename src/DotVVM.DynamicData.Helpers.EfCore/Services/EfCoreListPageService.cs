using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DotVVM.DynamicData.Helpers.Services;
using DotVVM.Framework.Controls;
using Microsoft.EntityFrameworkCore;

namespace DotVVM.DynamicData.Helpers.EfCore.Services
{
    public class EfCoreListPageService<TDbContext, TEntity, TListModel, TFilterModel> : IListPageService<TListModel, TFilterModel> 
        where TDbContext : DbContext
        where TEntity : class
    {
        private TDbContext DbContext { get; }

        private readonly Func<IQueryable<TEntity>, TFilterModel, IQueryable<TEntity>>? entityFilter;
        private readonly Func<IQueryable<TEntity>, IQueryable<TListModel>>? projection;
        private readonly Func<IGridViewDataSet<TListModel>, TDbContext, Task>? postProcessor;

        public EfCoreListPageService(
            TDbContext dbContext, 
            Func<IQueryable<TEntity>, TFilterModel, IQueryable<TEntity>>? entityFilter = null,
            Func<IQueryable<TEntity>, IQueryable<TListModel>>? projection = null,
            Func<IGridViewDataSet<TListModel>, TDbContext, Task>? postProcessor = null)
        {
            if (projection == null && typeof(TEntity) != typeof(TListModel))
            {
                throw new Exception($"The EfCoreService needs to specify mapping because the model type {typeof(TListModel)} is not the same as the entity type {typeof(TEntity)}.");
            }
            
            DbContext = dbContext;
            this.entityFilter = entityFilter;
            this.projection = projection;
            this.postProcessor = postProcessor;
        }

        public async Task LoadItems(IGridViewDataSet<TListModel> items, TFilterModel filter)
        {
            IQueryable<TEntity> queryable = DbContext.Set<TEntity>();

            if (entityFilter != null)
            {
                queryable = entityFilter(queryable, filter);
            }

            IQueryable<TListModel> mappedQueryable;
            if (projection != null)
            {
                mappedQueryable = projection(queryable);
            }
            else
            {
                mappedQueryable = (IQueryable<TListModel>)queryable;
            }

            await items.LoadFromQueryableAsync(mappedQueryable);

            if (postProcessor != null)
            {
                await postProcessor(items, DbContext);
            }
        }

    }
}
