namespace Oversee.ViewModels;

// Admin view of AppUsers
public class AppUser_AdminVM: AppUserVM

{
    public IEnumerable<string>? Roles { get; set; } // Show what roles a user has
}
