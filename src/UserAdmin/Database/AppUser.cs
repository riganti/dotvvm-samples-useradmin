using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace UserAdmin.Database;

public class AppUser : IdentityUser
{

    public virtual ICollection<AppUserRole> Roles { get; } = new List<AppUserRole>();

}