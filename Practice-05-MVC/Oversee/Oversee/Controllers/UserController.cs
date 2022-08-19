using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Oversee.Data;
using Oversee.Models;
using Oversee.Repository.IRepository;
using Oversee.ViewModels;

namespace Oversee.Controllers;

[Authorize(Roles = AccountRoles_SD.AppUser)] // Make sure to be able to perform CRUD on user roles
public class UserController : Controller
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly UserManager<AppUser> _userManager;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IMapper _mapper;

    public UserController(
        IUnitOfWork unitOfWork,
        UserManager<AppUser> userManager,
        IHttpContextAccessor httpContextAccessor,
        IMapper mapper
        )
    {
        _unitOfWork = unitOfWork;
        _userManager = userManager;
        _httpContextAccessor = httpContextAccessor;
        _mapper = mapper;
    }


    [HttpGet]
    public async Task<IActionResult> ViewUsers()
    {
        var currentUserId = _httpContextAccessor.HttpContext?.User.GetUserId();
        // Get list of users, excluding current user
        var users = _mapper.Map<List<AppUserVM>>(_userManager.Users.Where(u => u.Id != currentUserId).ToList());

        #region Sort user connection requests
        var userConnectionRequests_Sent = _unitOfWork.UserConnectionRequests.GetSome(u => u.SenderId == currentUserId);
        var userConnectionRequests_Received = _unitOfWork.UserConnectionRequests.GetSome(u => u.ReceiverId == currentUserId);

        var connectedUserRequests = new List<UserConnectionRequest>();
        var pendingUserRequests = new List<UserConnectionRequest>();
        var awaitingUserRequests = new List<UserConnectionRequest>();

        var receivedRequestsSet = new HashSet<UserConnectionRequest>();
        var connectedUserRequestIds = new HashSet<int>();


        foreach (var sentRequest in userConnectionRequests_Sent)
        {
            foreach (var receivedRequest in userConnectionRequests_Received)
            {
                if (receivedRequest.ReceiverId == sentRequest.SenderId) // If connection requests were sent to each other
                {
                    connectedUserRequests.Add(receivedRequest);
                    connectedUserRequests.Add(sentRequest);
                    
                    receivedRequestsSet.Add(receivedRequest); // Add to HashSet for when adding remaining received requests that haven't been matched (or use connectedUserRequests.Contains)
                    connectedUserRequestIds.Add(receivedRequest.Id);
                    connectedUserRequestIds.Add(sentRequest.Id);
                    break; // Connection requests matched, continue to next received request
                }
            }

            // If no matching request, add to pending users
            pendingUserRequests.Add(sentRequest);
        }

        // Sort the remaining unmatched received requests
        foreach (var receivedRequest in userConnectionRequests_Received)
        {
            if (!connectedUserRequestIds.Contains(receivedRequest.Id))
            {
                awaitingUserRequests.Add(receivedRequest);
            }
        }
        #endregion

        #region Extract and map user data from user connection requests
        var connnectedUsers = new List<AppUserVM>();
        var pendingUsers = new List<AppUserVM>();
        var awaitingUsers = new List<AppUserVM>();

        // connectedUsers: Create a filtered List of all unique user ids (who aren't the current user)

        // pending

        // awaiting

        #endregion

        return View(new ViewUsersVM
        {
            Users = users,
            ConnectedUsers = connnectedUsers,
            PendingUsers = pendingUsers,
            AwaitingUsers = awaitingUsers
        });
    }

    [HttpGet]
    public async Task<IActionResult> Profile(string id)
    {
        var profileUser = await _userManager.FindByIdAsync(id);
        if (profileUser == null)
        {
            TempData["error"] = "User not found";
            return RedirectToAction("Index", "Home"); // (change to Previous url)
        }

        var mappedUser = _mapper.Map<AppUserVM>(profileUser);
        //model.Roles = await _userManager.GetRolesAsync(currentUser); // Add role data

        return View(new ProfileVM
        {
            ProfileUser = mappedUser,
            CurrentUserId = _httpContextAccessor.HttpContext?.User.GetUserId()
        });
    }


    [HttpGet]
    public async Task<IActionResult> ProfileEdit(string id)
    {
        #region Demo admin cannot make user changes
        var currentUserEmail = _httpContextAccessor.HttpContext?.User.GetEmail();
        if (currentUserEmail.ToUpper() == "admin@example.com".ToUpper())
        {
            TempData["error"] = "Demo Admin not allowed to delete user data";
            return RedirectToAction("Index", "Home"); // (change to Previous url)
        }
        #endregion

        var user = await _userManager.FindByIdAsync(id);
        if (user == null)
        {
            TempData["error"] = "User not found";
            return RedirectToAction("Index", "Home"); // (change to Previous url)
        }

        var model = _mapper.Map<ProfileEditVM>(user); // Map user values to an edit form

        return View(model);
    }

    [HttpPost]
    public async Task<IActionResult> ProfileEdit(ProfileEditVM form)
    {
        if (!ModelState.IsValid)
            return View(form);

        #region Demo admin cannot make user changes
        var currentUserEmail = _httpContextAccessor.HttpContext?.User.GetEmail();
        if (currentUserEmail.ToUpper() == "admin@example.com".ToUpper())
        {
            TempData["error"] = "Demo Admin not allowed to delete user data";
            return RedirectToAction("Index", "Home"); // (change to Previous url)
        }
        #endregion


        var user = await _userManager.FindByIdAsync(form.Id);
        if (user == null)
        {
            TempData["error"] = "User not found";
            return RedirectToAction("Index", "Home"); // (change to Previous url)
        }

        user = _mapper.Map<ProfileEditVM, AppUser>(form, user);

        var updateResult = await _userManager.UpdateAsync(user);
        if (!updateResult.Succeeded)
        {
            TempData["error"] = "Server error";
            return RedirectToAction("Index", "Home"); // (change to Previous url)
        }

        return RedirectToAction("Index", "Home"); // (change to Previous url)
    }

    [HttpPost]
    [Authorize(Roles = AccountRoles_SD.Admin)]
    public async Task<IActionResult> ProfileEdit_Admin(ProfileEdit_AdminVM form)
    {
        if (!ModelState.IsValid)
            return View(form);

        var user = await _userManager.FindByIdAsync(form.Id);
        if (user == null)
        {
            TempData["error"] = "User not found";
            return RedirectToAction("Index", "Home"); // (change to Previous url)
        }

        user = _mapper.Map<ProfileEdit_AdminVM, AppUser>(form, user);

        var updateResult = await _userManager.UpdateAsync(user);
        if (!updateResult.Succeeded)
        {
            TempData["error"] = "Server error";
            return RedirectToAction("Index", "Home"); // (change to Previous url)
        }


        return RedirectToAction("Index", "Home"); // (change to Previous url)
    }
}
