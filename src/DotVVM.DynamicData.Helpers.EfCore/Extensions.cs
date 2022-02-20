using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DotVVM.Framework.Controls;
using Microsoft.EntityFrameworkCore;

namespace DotVVM.DynamicData.Helpers.EfCore
{
    public static class Extensions
    {

        public static async Task LoadFromQueryableAsync<T>(this IGridViewDataSet<T> dataSet, IQueryable<T> source)
        {
            var concreteDataSet = (GridViewDataSet<T>)dataSet;  // TODO why the methods are not on the interface?
            source = concreteDataSet.ApplyFilteringToQueryable(source);
            concreteDataSet.Items = await concreteDataSet.ApplyOptionsToQueryable(source).ToListAsync();
            concreteDataSet.PagingOptions.TotalItemsCount = await source.CountAsync();
            concreteDataSet.IsRefreshRequired = false;
        }

    }
}
