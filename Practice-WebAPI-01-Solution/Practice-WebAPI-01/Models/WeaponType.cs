using System.ComponentModel.DataAnnotations;

namespace Practice_WebAPI_01.Models;

public class WeaponType
{
    public int Id { get; set; }
    [Required]
    [MaxLength(100)]
    public string Type { get; set; }
}
