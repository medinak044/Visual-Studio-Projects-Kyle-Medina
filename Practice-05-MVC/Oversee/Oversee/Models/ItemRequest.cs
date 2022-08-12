using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Oversee.Models;

// Similar to UserConnectionRequest, must have 2 related ItemRequests to imply Item is now borrowed
public class ItemRequest
{
    public int Id { get; set; }
    public bool ReturnConfirmed { get; set; }
    [ForeignKey("Item")]
    public int ItemId { get; set; }
    public Item? Item { get; set; }
    [ForeignKey("AppUser")]
    public string AppUserId { get; set; }
    public AppUser? AppUser { get; set; }
}
