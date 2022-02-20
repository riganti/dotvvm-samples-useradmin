using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DotVVM.DynamicData.Helpers.ViewModels;
using DotVVM.Framework.ViewModel;
using UserAdmin.Services.Roles;

namespace UserAdmin.ViewModels.Roles
{
    public class ListViewModel : UserAdmin.ViewModels.MasterPageViewModel
    {
        public ListPageViewModel<RoleModel, RoleFilterModel> Page { get; set; }

        public ListViewModel(RoleService service)
        {
            Page = new ListPageViewModel<RoleModel, RoleFilterModel>(service);
        }
    }
}

