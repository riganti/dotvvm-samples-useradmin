using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DotVVM.DynamicData.Helpers.ViewModels;
using DotVVM.Framework.ViewModel;
using UserAdmin.Services.Users;

namespace UserAdmin.ViewModels.Users;

public class ListViewModel : UserAdmin.ViewModels.MasterPageViewModel
{

    public ListPageViewModel<UserListModel, UserFilterModel> Page { get; set; }

    public ListViewModel(UserService service)
    {
        Page = new ListPageViewModel<UserListModel, UserFilterModel>(service);
    }

}