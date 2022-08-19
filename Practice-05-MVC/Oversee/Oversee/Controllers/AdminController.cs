using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Oversee.Data;
using Oversee.Models;
using Oversee.ViewModels;

namespace Oversee.Controllers;

[Authorize(Roles = AccountRoles_SD.Admin)]
public class AdminController : Controller
{
    private readonly UserManager<AppUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IMapper _mapper;

    public AdminController(
        UserManager<AppUser> userManager,
        RoleManager<IdentityRole> roleManager,
        IHttpContextAccessor httpContextAccessor,
        IMapper mapper
        )
    {
        _userManager = userManager;
        _roleManager = roleManager;
        _httpContextAccessor = httpContextAccessor;
        _mapper = mapper;
    }


    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var currentUserId = _httpContextAccessor.HttpContext?.User.GetUserId();
        var currentUser = await _userManager.FindByIdAsync(currentUserId);
        var users = new List<AppUser>();
        users.Add(currentUser); // Add current user first
        users.AddRange(_userManager.Users.Where(u => u.Id != currentUserId)); // Add the rest of the users

        var model = new List<AppUser_AdminVM>();

        // Map respective roles for each user
        foreach (var user in users)
        {
            var vm = _mapper.Map<AppUser_AdminVM>(user);
            vm.Roles = await _userManager.GetRolesAsync(user); // Add role data
            model.Add(vm);
        }

        return View(model);
    }


    [HttpGet]
    public async Task<IActionResult> Delete(string id)
    {
        #region Demo admin cannot make user changes
        var currentUserEmail = _httpContextAccessor.HttpContext?.User.GetEmail();
        if (currentUserEmail.ToUpper() == "admin@example.com".ToUpper())
        {
            TempData["error"] = "Demo Admin not allowed to delete user data";
            return RedirectToAction("Index", "Admin");
        }
        #endregion

        var user = await _userManager.FindByIdAsync(id);
        if (user == null)
        {
            TempData["error"] = "User not found";
            return RedirectToAction("Index", "Admin"); // (change to Previous url)
        }

        var deleteResult = await _userManager.DeleteAsync(user);
        if (!deleteResult.Succeeded)
        {
            TempData["error"] = "Server error";
            return RedirectToAction("Index", "Admin"); // (change to Previous url)
        }

        return RedirectToAction("Index", "Admin");
    }
}
