﻿using System.ComponentModel.DataAnnotations;

namespace Oversee.ViewModels;

public class LoginVM
{
    [Display(Name = "Email")]
    [Required(ErrorMessage = "Email is required.")]
    public string Email { get; set; }
    [Required]
    [DataType(DataType.Password)]
    public string Password { get; set; }
}
