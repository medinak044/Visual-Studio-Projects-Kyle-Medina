using Oversee.Models;

namespace Oversee.ViewModels;

// Data sent back to client (DTO)/ View Model
public class UserConnectionVM
{
    public IEnumerable<AppUser>? ConnectedUsers { get; set; } // Should contain 1 or 2 users
}
