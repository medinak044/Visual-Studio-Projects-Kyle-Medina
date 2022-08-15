using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Oversee.Data;
using Oversee.Models;
using Oversee.ViewModels;

namespace Oversee.Controllers
{
    //[Authorize(Roles = AccountRoles_SD.AppUser)] // Make sure to be able to perform CRUD on user roles
    public class UserController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IMapper _mapper;

        public UserController(
            UserManager<AppUser> userManager,
            IHttpContextAccessor httpContextAccessor,
            IMapper mapper
            )
        {
            _userManager = userManager;
            _httpContextAccessor = httpContextAccessor;
            _mapper = mapper;
        }


        [HttpGet]
        public async Task<IActionResult> ViewUsers()
        {
            // Get all users and display list of users (excluding current user)

            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Profile(string id)
        {
            var currentUserId = _httpContextAccessor.HttpContext?.User.GetUserId(); // Get current user's id (cookie)
            var currentUser = await _userManager.FindByIdAsync(currentUserId);

            if (id == currentUserId)
            {

            }
            var model = _mapper.Map<AppUser_AdminVM>(currentUser);
            model.Roles = await _userManager.GetRolesAsync(currentUser); // Add role data

            return View(model);
        }


        [HttpGet]
        public async Task<IActionResult> ProfileEdit(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                TempData["Error"] = "User not found";
                return RedirectToAction("Index", "Home"); // (change to Previous url)
            }

            var model = _mapper.Map<ProfileEditVM>(user); // Map user values to an edit form

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ProfileEdit(ProfileEditVM form)
        {
            if (!ModelState.IsValid)
                return View(form);

            var user = await _userManager.FindByIdAsync(form.Id);
            if (user == null)
            {
                TempData["Error"] = "User not found";
                return RedirectToAction("Index", "Home"); // (change to Previous url)
            }

            user = _mapper.Map<ProfileEditVM, AppUser>(form, user);

            var updateResult = await _userManager.UpdateAsync(user);
            if (!updateResult.Succeeded)
            {
                TempData["Error"] = "Server error";
                return RedirectToAction("Index", "Home"); // (change to Previous url)
            }

            return RedirectToAction("Index", "Home"); // (change to Previous url)
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ProfileEdit_Admin(ProfileEdit_AdminVM form)
        {
            if (!ModelState.IsValid)
                return View(form);

            var user = await _userManager.FindByIdAsync(form.Id);
            if (user == null)
            {
                TempData["Error"] = "User not found";
                return RedirectToAction("Index", "Home"); // (change to Previous url)
            }

            user = _mapper.Map<ProfileEdit_AdminVM, AppUser>(form, user);

            var updateResult = await _userManager.UpdateAsync(user);
            if (!updateResult.Succeeded)
            {
                TempData["Error"] = "Server error";
                return RedirectToAction("Index", "Home"); // (change to Previous url)
            }


            return RedirectToAction("Index", "Home"); // (change to Previous url)
        }
    }
}
