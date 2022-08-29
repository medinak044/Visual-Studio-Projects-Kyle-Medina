namespace Oversee.Models;

// If 2 UserConnectionRequest objects are found containing each user's id, it's implied that they are connected
// 1 UserConnectionRequest is an invite to connect
public class UserConnectionRequest
{
    public int Id { get; set; }
    public string SendingUserId { get; set; }
    public AppUser? SendingUser { get; set; }
    public string ReceivingUserId { get; set; }
    public AppUser? ReceivingUser { get; set; }
    public bool IsConnected { get; set; }
}
