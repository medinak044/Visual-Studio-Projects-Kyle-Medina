namespace Oversee.ViewModels;

public class ViewUsersVM
{
    public string? CurrentUserId { get; set; }
    public List<AppUserVM>? Users { get; set; } = new List<AppUserVM>();
    public List<AppUserVM>? ConnectedUsers { get; set; } = new List<AppUserVM>(); // Users who are confirmed a connection
    public List<AppUserVM>? PendingUsers { get; set; } = new List<AppUserVM>(); // Users whom current user sent a request to
    public List<AppUserVM>? AwaitingUsers { get; set; } = new List<AppUserVM>(); // Users who sent current user a request 
}
