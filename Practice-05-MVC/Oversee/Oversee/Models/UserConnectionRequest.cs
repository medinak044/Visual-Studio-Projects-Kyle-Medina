namespace Oversee.Models;

// If 2 UserConnectionRequest objects are found containing each user's id, it's implied that they are connected
// 1 UserConnectionRequest is an invite to connect
public class UserConnectionRequest
{
    public int Id { get; set; }
    public string SenderId { get; set; }
    public string ReceiverId { get; set; }
}
