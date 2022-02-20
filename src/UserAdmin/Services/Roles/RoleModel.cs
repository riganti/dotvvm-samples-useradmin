using System.ComponentModel.DataAnnotations;

namespace UserAdmin.Services.Roles;

public class RoleModel
{
    [Display(AutoGenerateField = false)]
    public string Id { get; set; }

    public string Name { get; set; }

}