using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using DotVVM.DynamicData.Helpers.Services;
using Humanizer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace DotVVM.DynamicData.Helpers.EfCore.Services
{
    public class EfCoreDetailPageService<TDbContext, TEntity, TModel, TKey> : IDetailPageService<TModel, TKey>
        where TDbContext : DbContext 
        where TEntity : class, new()
        where TModel : new()
    {
        public TDbContext DbContext { get; }

        private readonly Func<IQueryable<TEntity>, IQueryable<TEntity>>? entityFilter;
        private readonly Func<TEntity, TModel>? map;
        private readonly Action<TEntity, TModel>? mapBack;

        public EfCoreDetailPageService(
            TDbContext dbContext,
            Func<IQueryable<TEntity>, IQueryable<TEntity>>? entityFilter = null,
            Func<TEntity, TModel>? map = null,
            Action<TEntity, TModel>? mapBack = null)
        {
            this.entityFilter = entityFilter;
            this.map = map;
            this.mapBack = mapBack;

            DbContext = dbContext;

            // TODO: async mapping
        }

        public Task<TModel> LoadItem(TKey id)
        {
            var entity = GetEntityById(id);

            TModel model;
            if (map != null)
            {
                model = map(entity);
            }
            else if (entity is TModel convertedEntity)
            {
                model = convertedEntity;
            }
            else
            {
                throw new Exception($"Mapping must be specified in EfCoreDetailPageService if the entity type {typeof(TEntity)} is not the same as the model type {typeof(TEntity)}!");
            }

            return Task.FromResult(model);
        }

        public Task<TModel> InitializeItem()
        {
            // TODO: custom initialization
            return Task.FromResult(new TModel());
        }

        public async Task SaveItem(TModel item, TKey id)
        {
            TEntity entity;
            if (mapBack != null)
            {
                if (Equals(id, default(TKey)))
                {
                    entity = new TEntity();
                    DbContext.Set<TEntity>().Add(entity);
                }
                else
                {
                    entity = GetEntityById(id);
                }

                mapBack(entity, item);
            }
            else if (item is TEntity convertedEntity)
            {
                if (Equals(id, default(TKey)))
                {
                    DbContext.Set<TEntity>().Add(convertedEntity);
                }
                else
                {
                    DbContext.Set<TEntity>().Attach(convertedEntity);
                    DbContext.Entry(convertedEntity).State = EntityState.Modified;
                }
            }
            else
            {
                throw new Exception($"Mapping must be specified in EfCoreDetailPageService if the entity type {typeof(TEntity)} is not the same as the model type {typeof(TEntity)}!");
            }

            await DbContext.SaveChangesAsync();
        }

        public async Task DeleteItem(TKey id)
        {
            // TODO: soft delete

            var entity = GetEntityById(id);
            DbContext.Set<TEntity>().Remove(entity);

            await DbContext.SaveChangesAsync();
        }

        protected virtual IQueryable<TEntity> GetQuery()
        {
            IQueryable<TEntity> query = DbContext.Set<TEntity>();
            if (entityFilter != null)
            {
                query = entityFilter(query);
            }
            return query;
        }

        private TEntity GetEntityById(TKey id)
        {
            var query = GetQuery();

            // TODO: caching and validation
            var key = DbContext.Model.FindEntityType(typeof(TEntity))!.FindPrimaryKey()!;
            var keyProp = key.Properties.Single().PropertyInfo!;

            var param = Expression.Parameter(typeof(TEntity));
            var body = Expression.Equal(Expression.Property(param, keyProp), Expression.Constant(id));
            
            return query.Single(Expression.Lambda<Func<TEntity, bool>>(body, param));
        }

    }
}
