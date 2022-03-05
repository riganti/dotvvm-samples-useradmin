using DotVVM.DynamicData.Helpers.Context;

namespace DotVVM.DynamicData.Helpers.Configuration.Builders;

public interface IDetailServiceBuilderWithMapping<TEntity, TModel>
{

    IDetailServiceBuilderWithMapping<TEntity, TModel> UseMapping(Func<TEntity, ServiceContext, TModel> map, Action<TEntity, TModel, ServiceContext> mapBack);

}