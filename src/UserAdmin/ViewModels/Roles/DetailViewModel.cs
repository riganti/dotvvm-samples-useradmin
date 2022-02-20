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
    public class DetailViewModel : UserAdmin.ViewModels.MasterPageViewModel
    {
        public DetailPageViewModel<RoleModel, string> Page { get; set; }

        public DetailViewModel(RoleService service)
        {
            Page = new DetailPageViewModel<RoleModel, string>(service);
            Page.ReturnToListPageRequested += () => Context.RedirectToRoute("Roles_List");
        }
    }
}

