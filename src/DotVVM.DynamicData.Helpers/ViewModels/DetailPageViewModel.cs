using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DotVVM.DynamicData.Helpers.Services;
using DotVVM.Framework.ViewModel;

namespace DotVVM.DynamicData.Helpers.ViewModels
{
    public class DetailPageViewModel<TDetailModel, TKey> : DynamicPageViewModelBase 
    {
        private readonly IDetailPageService<TDetailModel, TKey> service;

        public TDetailModel? Item { get; set; }

        [FromRoute("Value")]
        public TKey Id { get; set; }

        public event Action? ReturnToListPageRequested;
        
        public DetailPageViewModel(IDetailPageService<TDetailModel, TKey> service)
        {
            this.service = service;
        }

        public override async Task PreRender()
        {
            if (!Context.IsPostBack)
            {
                if (Equals(Id, default(TKey)))
                {
                    Item = await service.InitializeItem();
                }
                else
                {
                    Item = await service.LoadItem(Id);
                }
            }

            await base.PreRender();
        }

        public virtual async Task Save()
        {
            await service.SaveItem(Item!, Id);

            ReturnToListPageRequested?.Invoke();
        }

        public virtual async Task Cancel()
        {
            ReturnToListPageRequested?.Invoke();
        }

        public virtual async Task Delete()
        {
            await service.DeleteItem(Id);

            ReturnToListPageRequested?.Invoke();
        }

    }
}
