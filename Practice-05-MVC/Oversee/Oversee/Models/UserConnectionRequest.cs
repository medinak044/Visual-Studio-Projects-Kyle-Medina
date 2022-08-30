namespace Oversee.Models;

// If 2 UserConnectionRequest_Users objs are found, it's implied that they are connected
public class UserConnectionRequest
{
    public int Id { get; set; }
    public string SendingUserId { get; set; }
    public AppUser? SendingUser { get; set; }
    public string ReceivingUserId { get; set; }
    public AppUser? ReceivingUser { get; set; }
    public bool IsConnected { get; set; } // (Might consider removing since a comparison can be made using UserConnectionRequest_Users instead)
}
