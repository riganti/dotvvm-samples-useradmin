using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using DotVVM.DynamicData.Helpers.Configuration.Builders;
using Microsoft.Extensions.DependencyInjection;

// ReSharper disable once CheckNamespace
namespace DotVVM.DynamicData.Helpers
{
    public static class BuilderExtensions
    {

        public static T UseAutoMapper<T, TEntity, TModel>(this T builder)
            where T : IListServiceBuilderWithMapping<TEntity, TModel>
        {
            return (T)builder.UseMapping((entities, context) => context.DotvvmRequestContext.Services.GetRequiredService<IMapper>().ProjectTo<TModel>(entities));
        }

        public static T UseMapping<T, TEntity, TModel>(this T builder)
            where T : IDetailServiceBuilderWithMapping<TEntity, TModel>
        {
            return (T)builder.UseMapping(
                (entity, context) => context.DotvvmRequestContext.Services.GetRequiredService<IMapper>().Map<TModel>(entity),
                (entity, model, context) => context.DotvvmRequestContext.Services.GetRequiredService<IMapper>().Map(model, entity)
            );
        }

    }
}
