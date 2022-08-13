using System.ComponentModel.DataAnnotations;

namespace Oversee.ViewModels;

public class RegisterVM
{

    [Display(Name = "First name")]
    [Required(ErrorMessage = "First name is required")]
    public string FirstName { get; set; }
    [Display(Name = "Last name")]
    [Required(ErrorMessage = "Last name is required")]
    public string LastName { get; set; }
    [Display(Name = "User name")]
    [Required(ErrorMessage = "User name is required")]
    public string UserName { get; set; }
    [Display(Name = "Email")]
    [Required(ErrorMessage = "Email is required")]
    public string Email { get; set; }
    [Required]
    [DataType(DataType.Password)]
    public string Password { get; set; }
    [Display(Name = "Confirm password")]
    [Required(ErrorMessage = "Must confirm password")]
    [DataType(DataType.Password)]
    [Compare("Password", ErrorMessage = "Passwords do not match")]
    public string ConfirmPassword { get; set; }
}
