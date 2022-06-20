using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;

namespace Practice_WebAPI_01.Models;

public class Hero
{
    public int Id { get; set; }
    [Required]
    [MaxLength(100)]
    public string UserName { get; set; }
    public int Credit { get; set; }
    //public DateTime Created { get; set; } = DateTime.Now;
    [ValidateNever]
    public ICollection<Weapon> Weapons { get; set; }
}
