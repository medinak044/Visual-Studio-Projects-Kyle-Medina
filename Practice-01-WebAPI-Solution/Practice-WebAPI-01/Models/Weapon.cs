using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;

namespace Practice_WebAPI_01.Models;

public class Weapon
{
    public int Id { get; set; }
    [Required]
    [MaxLength(100)]
    public string Name { get; set; }
    [Required]
    [ValidateNever]
    public WeaponType WeaponType { get; set; }
    //public int HeroUserId { get; set; }
}
