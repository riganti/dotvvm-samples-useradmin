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
    public abstract class EfCoreListPageService<TEntity, TListModel, TFilterModel> : IListPageService<TListModel, TFilterModel> 
        where TEntity : class
    {
        protected readonly DbContext dbContext;

        public EfCoreListPageService(DbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task LoadItems(IGridViewDataSet<TListModel> items, TFilterModel? filter)
        {
            await ValidateQuery(items, filter);

            IQueryable<TEntity> queryable = dbContext.Set<TEntity>();
            queryable = ApplyFilter(queryable, filter);
            var mappedQueryable = await ProjectAsync(queryable);
            await items.LoadFromQueryableAsync(mappedQueryable);

            await PostProcessResults(items);
        }

        protected abstract Task<IQueryable<TListModel>> ProjectAsync(IQueryable<TEntity> queryable);

        protected virtual Task PostProcessResults(IGridViewDataSet<TListModel> items)
        {
            return Task.CompletedTask;
        }

        protected virtual Task ValidateQuery(IGridViewDataSet<TListModel> items, TFilterModel? filter)
        {
            return Task.CompletedTask;
        }

        protected IQueryable<TEntity> ApplyFilter(IQueryable<TEntity> queryable, TFilterModel? filter)
        {
            return queryable;
        }
    }
}
