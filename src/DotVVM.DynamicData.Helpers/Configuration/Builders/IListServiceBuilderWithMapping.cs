using DotVVM.DynamicData.Helpers.Context;

namespace DotVVM.DynamicData.Helpers.Configuration.Builders;

public interface IListServiceBuilderWithMapping<TEntity, TModel>
{

    IListServiceBuilderWithMapping<TEntity, TModel> UseMapping(Func<IQueryable<TEntity>, ServiceContext, IQueryable<TModel>> projection);

}