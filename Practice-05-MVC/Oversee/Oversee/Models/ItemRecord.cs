using System.ComponentModel.DataAnnotations.Schema;

namespace Oversee.Models;

public class ItemRecord
{
    public int Id { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public bool ReturnConfirmed { get; set; } // Set to true only if both ItemRequests have confirmed
    [ForeignKey("Item")]
    public int ItemId { get; set; }
    public Item? Item { get; set; }
    public ICollection<ItemRecordUser>? InvolvedUsers { get; set; }
}
