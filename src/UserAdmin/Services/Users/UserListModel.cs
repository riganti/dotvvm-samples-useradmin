using System.ComponentModel.DataAnnotations;
using UserAdmin.Resources;

namespace UserAdmin.Services.Users;

public class UserListModel
{
    [Display(AutoGenerateField = false)]
    public string Id { get; set; }

        [Display(ResourceType = typeof(PropertyNames), Name = "UserName")]
    public string UserName { get; set; }

    public string Email { get; set; }

    public string PhoneNumber { get; set; }

    public string Roles { get; set; }
}