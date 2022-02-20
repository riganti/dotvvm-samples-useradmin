using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using DotVVM.Framework.Controls.DynamicData.Annotations;
using UserAdmin.Resources;
using UserAdmin.Services.Roles;

namespace UserAdmin.Services.Users;

public class UserDetailModel
{
    public string UserName { get; set; }

    public string Email { get; set; }

    public string PhoneNumber { get; set; }

    [Selector(typeof(RoleSelectorItem))]
    public List<string> Roles { get; set; }
}