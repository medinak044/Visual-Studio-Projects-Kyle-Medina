using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Oversee.Models;

namespace Oversee.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;

        public AccountController(
            UserManager<AppUser> userManager,
            SignInManager<AppUser> signInManager
            )
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public async Task<ActionResult> Index()
        {
            return View();
        }



    }
}
