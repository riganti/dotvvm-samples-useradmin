using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DotVVM.DynamicData.Helpers.ViewModels;
using DotVVM.Framework.Controls.DynamicData.ViewModel;
using DotVVM.Framework.ViewModel;
using UserAdmin.Services.Roles;
using UserAdmin.Services.Users;

namespace UserAdmin.ViewModels.Users;

public class DetailViewModel : MasterPageViewModel
{
    public SelectorViewModel<RoleSelectorItem> Roles { get; set; } = new();

    public DetailPageViewModel<UserDetailModel, string> Page { get; set; }

    public DetailViewModel(UserDetailService service)
    {
        Page = new DetailPageViewModel<UserDetailModel, string>(service);
        Page.ReturnToListPageRequested += () => Context.RedirectToRoute("Users_List");
    }

}