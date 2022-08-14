using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Oversee.Data;
using Oversee.Models;

namespace Oversee.Controllers
{
    [Authorize(Roles = AccountRoles_SD.Admin)]
    public class AdminController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IMapper _mapper;

        public AdminController(
            UserManager<AppUser> userManager,
            IHttpContextAccessor httpContextAccessor,
            IMapper mapper
            )
        {
            _userManager = userManager;
            _httpContextAccessor = httpContextAccessor;
            _mapper = mapper;
        }


        // GET: AdminController
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var currentUser_Memory = _httpContextAccessor.HttpContext?.User; // Get current user
            var users = _userManager.Users.Where(u => u.Id != currentUser_Memory.ToString()); // Exclude current user
            //var users = await _userManager.Users.ToListAsync(); // Exclude current user
            var currentUser = await _userManager.FindByIdAsync(currentUser_Memory.ToString());
            var result = new List<AppUser>();
            result.Add(currentUser); // Add current user first
            result.AddRange(users); // Add the rest of the users
            return View(result);
        }

        // GET: AdminController/Details/5
        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            return View();
        }

        // Creating AppUsers handled by UserManager in AccountController
        //// GET: AdminController/Create
        //[HttpGet]
        //public ActionResult Create()
        //{
        //    return View();
        //}

        //// POST: AdminController/Create
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Create(IFormCollection collection)
        //{
        //    try
        //    {
        //        return RedirectToAction(nameof(Index));
        //    }
        //    catch
        //    {
        //        return View();
        //    }
        //}

        // GET: AdminController/Edit/5
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            return View();
        }

        // POST: AdminController/Edit/5
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: AdminController/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            return View();
        }

        // POST: AdminController/Delete/5
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
