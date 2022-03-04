using Microsoft.AspNetCore.Identity;

namespace UserAdmin.Database;

public class AppUserRole : IdentityUserRole<string>
{
    public AppRole Role { get; set; }
    public AppUser User { get; set; }
}