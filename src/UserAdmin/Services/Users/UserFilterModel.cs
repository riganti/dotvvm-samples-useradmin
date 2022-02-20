namespace UserAdmin.Services.Users;

public record UserFilterModel
{
    public string Search { get; set; } = "";
}