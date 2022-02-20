using DotVVM.Framework.Controls;

namespace DotVVM.DynamicData.Helpers.Services;

public interface IListPageService<TItem, TFilter>
{

    Task LoadItems(IGridViewDataSet<TItem> items, TFilter? filter);

}