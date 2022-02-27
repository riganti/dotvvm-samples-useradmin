using System;
using System.Collections.Generic;
using System.Linq;
using DotVVM.DynamicData.Helpers.ViewModels;
using DotVVM.Framework.ViewModel;

namespace UserAdmin.ViewModels
{
    public class DynamicPageViewModelHost<TPageViewModel, TSelectorsTuple> : MasterPageViewModel
        where TPageViewModel : DotvvmViewModelBase
    {

        public TSelectorsTuple Selectors { get; set; } = DynamicPageViewModelBase.InitSelectors<TSelectorsTuple>();

        public TPageViewModel Page { get; set; }

        public DynamicPageViewModelHost(TPageViewModel page)
        {
            Page = page;
        }

        protected override IEnumerable<IDotvvmViewModel> GetChildViewModels()
        {
            return base.GetChildViewModels().Concat(DynamicPageViewModelBase.GetSelectorViewModels(Selectors));
        }
    }
}
