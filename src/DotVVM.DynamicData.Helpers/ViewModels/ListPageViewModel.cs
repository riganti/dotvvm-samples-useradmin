using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DotVVM.DynamicData.Helpers.Services;
using DotVVM.Framework.Controls;

namespace DotVVM.DynamicData.Helpers.ViewModels
{
    public class ListPageViewModel<TItem, TFilter> : DynamicPageViewModelBase
        where TFilter : new()
    {
        private readonly IListPageService<TItem, TFilter> service;

        public TFilter? Filter { get; set; } = new();

        public GridViewDataSet<TItem> Items { get; set; } = new()
        {
            PagingOptions =
            {
                PageSize = 50
            }
        };

        public ListPageViewModel(IListPageService<TItem, TFilter> service)
        {
            this.service = service;
        }

        public override async Task PreRender()
        {
            if (!Context.IsPostBack)
            {
                await InitializeDataSetAndFilters();
            }

            if (Items.IsRefreshRequired)
            {
                await service.LoadItems(Items, Filter);
            }

            await base.PreRender();
        }

        protected virtual Task InitializeDataSetAndFilters()
        {
            return Task.CompletedTask;
        }
        
    }
}
