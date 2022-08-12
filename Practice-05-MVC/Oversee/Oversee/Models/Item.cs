using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Oversee.Models;

public class Item
{
    public int Id { get; set; }
    [Required]
    public string Name { get; set; }
    public string? Description { get; set; }
    [ForeignKey("AppUser")]
    public string AppUserId { get; set; }
    public AppUser? AppUser { get; set; }
}
