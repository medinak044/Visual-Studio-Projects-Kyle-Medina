using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Oversee.Data;
using Oversee.Models;
using Oversee.ViewModels;

namespace Oversee.Controllers;

public class AccountController : Controller
{
    private readonly UserManager<AppUser> _userManager;
    private readonly SignInManager<AppUser> _signInManager;
    private readonly IMapper _mapper;

    public AccountController(
        UserManager<AppUser> userManager,
        SignInManager<AppUser> signInManager,
        IMapper mapper
        )
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _mapper = mapper;
    }

    [HttpGet]
    public async Task<IActionResult> Login()
    {
        var model = new LoginVM();
        return View(model);
    }

    [HttpPost]
    public async Task<IActionResult> Login(LoginVM loginVM)
    {
        if (!ModelState.IsValid)
            return View(loginVM);

        // Check if user exists
        var existingUser = await _userManager.FindByEmailAsync(loginVM.Email);
        if (existingUser == null)
        {
            TempData["Error"] = "Email doesn't exist";
            return View(loginVM);
        }

        // Verify password
        var passwordIsCorrect = await _userManager.CheckPasswordAsync(existingUser, loginVM.Password);
        if (!passwordIsCorrect)
        {
            TempData["Error"] = "Invalid credentials";
            return View(loginVM);
        }

        // Log user in
        var signInResult = await _signInManager.PasswordSignInAsync(existingUser, loginVM.Password, false, false);
        if (!signInResult.Succeeded)
        {
            TempData["Error"] = "Invalid credentials";
            return View(loginVM);
        }

        return RedirectToAction("Index", "Home");
    }

    [HttpGet]
    public async Task<IActionResult> Register()
    {
        var model = new RegisterVM();
        return View(model);
    }

    [HttpPost]
    public async Task<IActionResult> Register(RegisterVM registerVM)
    {
        if (!ModelState.IsValid)
            return View(registerVM);

        // Check if email already exists
        var existingUser = await _userManager.FindByEmailAsync(registerVM.Email);
        if (existingUser != null)
        {
            TempData["Error"] = "This email is already in use";
            return View(registerVM);
        }

        // Map values
        var newUser = _mapper.Map<AppUser>(registerVM);

        var newUserResponse = await _userManager.CreateAsync(newUser, registerVM.Password);
        if (!newUserResponse.Succeeded)
        {
            TempData["Error"] = "Server error";
            return View(registerVM);
        }

        // Assign default role to new user (Make sure roles exist in database first)
        await _userManager.AddToRoleAsync(newUser, AccountRoles_SD.AppUser);

        // Log user in as a convenience
        var user = await _userManager.FindByEmailAsync(registerVM.Email); // Track new user from db
        var isSignedIn = await _signInManager.PasswordSignInAsync(user, registerVM.Password, false, false);
        if (!isSignedIn.Succeeded)
        {
            TempData["Error"] = "Something went wrong while logging in. Please try again";
            return RedirectToAction("Login");
        }

        return RedirectToAction("Index", "Home");
    }

    [HttpGet]
    public async Task<IActionResult> Logout()
    {
        await _signInManager.SignOutAsync();
        return RedirectToAction("Index", "Home");
    }
}
