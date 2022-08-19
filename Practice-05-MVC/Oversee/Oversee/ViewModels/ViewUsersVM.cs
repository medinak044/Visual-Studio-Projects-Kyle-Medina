namespace Oversee.ViewModels;

public class ViewUsersVM
{
    public List<AppUserVM>? Users { get; set; }
    public List<AppUserVM>? ConnectedUsers { get; set; } // Users who are confirmed a connection
    public List<AppUserVM>? PendingUsers { get; set; } // Users whom current user sent a request to
    public List<AppUserVM>? AwaitingUsers { get; set; } // Users who sent current user a request 
}
