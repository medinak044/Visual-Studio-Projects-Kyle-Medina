using System.ComponentModel.DataAnnotations;

namespace Practice_WebAPI_01.Models
{
    public class Weapon
    {
        public int Id { get; set; }
        [Required]
        [MaxLength(100)]
        public string Name { get; set; }
        public WeaponType WeaponType { get; set; }
    }
}
