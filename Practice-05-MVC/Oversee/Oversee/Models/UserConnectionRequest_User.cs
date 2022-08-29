using System.ComponentModel.DataAnnotations.Schema;

namespace Oversee.Models;

public class UserConnectionRequest_User
{
    public int Id { get; set; }
    //[ForeignKey("AppUser")]
    public string UserId { get; set; }
    //[ForeignKey("UserConnectionRequest")]
    public int UserConnectionRequestId { get; set; }
}
