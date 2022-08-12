using System.ComponentModel.DataAnnotations.Schema;

namespace Oversee.Models;

public class ItemRecordUser // For ItemRecord only
{
    public int Id { get; set; }
    [ForeignKey("ItemRecord")]
    public int ItemRecordId { get; set; }
    [ForeignKey("AppUser")]
    public string AppUserId { get; set; }
    public AppUser? AppUser { get; set; }
}
