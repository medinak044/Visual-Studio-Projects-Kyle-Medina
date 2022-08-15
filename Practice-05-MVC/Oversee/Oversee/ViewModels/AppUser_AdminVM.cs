using Oversee.Models;

namespace Oversee.ViewModels;

// Admin view of AppUsers
public class AppUser_AdminVM: AppUser
{
    public IEnumerable<string>? Roles { get; set; } // Show what roles a user has
}
