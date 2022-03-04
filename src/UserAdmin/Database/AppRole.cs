using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace UserAdmin.Database;

public class AppRole : IdentityRole
{
    public ICollection<AppUserRole> UserRoles { get; } = new List<AppUserRole>();
}