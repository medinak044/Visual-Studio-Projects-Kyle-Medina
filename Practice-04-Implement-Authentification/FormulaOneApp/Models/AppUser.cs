using Microsoft.AspNetCore.Identity;

namespace FormulaOneApp.Models;

public class AppUser: IdentityUser
{
    public string RefreshToken { get; set; }
    public DateTime TokenCreated { get; set; }
    public DateTime TokenExpires { get; set; }

}
