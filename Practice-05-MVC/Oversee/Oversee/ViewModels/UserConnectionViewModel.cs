using Oversee.Models;

namespace Oversee.ViewModels;

// Data sent back to client (DTO)/ View Model
public class UserConnectionViewModel
{
    public ICollection<AppUser>? ConnectedUsers { get; set; } // Should contain 1 or 2 users
}
