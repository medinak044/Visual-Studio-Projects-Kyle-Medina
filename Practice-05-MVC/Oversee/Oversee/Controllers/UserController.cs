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
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IMapper _mapper;

    public UserController(
        IUnitOfWork unitOfWork,
        UserManager<AppUser> userManager,
        RoleManager<IdentityRole> roleManager,
        IHttpContextAccessor httpContextAccessor,
        IMapper mapper
        )
    {
        _unitOfWork = unitOfWork;
        _userManager = userManager;
        _roleManager = roleManager;
        _httpContextAccessor = httpContextAccessor;
        _mapper = mapper;
    }


    [HttpGet]
    public async Task<IActionResult> ViewUsers()
    {
        var currentUserId = _httpContextAccessor.HttpContext?.User.GetUserId();

        var users = _mapper.Map<List<AppUserVM>>(await _userManager.Users.ToListAsync()); // Exclude current user from list in View file
                                                                                          //var users = _mapper.Map<List<AppUserVM>>(_userManager.Users.Where(u => u.Id != currentUserId).ToList()); // Get list of users, excluding current user

        #region Sort user connection requests 01
        //var userConnectionRequests_Sent = _unitOfWork.UserConnectionRequests.GetSome(u => u.SendingUserId == currentUserId);
        //var userConnectionRequests_Received = _unitOfWork.UserConnectionRequests.GetSome(u => u.ReceivingUserId == currentUserId);

        //var connectedUserRequests = new List<UserConnectionRequest>();
        //var pendingUserRequests = new List<UserConnectionRequest>();
        //var awaitingUserRequests = new List<UserConnectionRequest>();

        //var connectedUserRequestIds = new HashSet<int>();

        //// Find matching sent/received requests
        //foreach (var sentRequest in userConnectionRequests_Sent)
        //{
        //    foreach (var receivedRequest in userConnectionRequests_Received)
        //    {
        //        if (receivedRequest.ReceivingUserId == sentRequest.SendingUserId) // If connection requests were sent to each other
        //        {
        //            connectedUserRequests.Add(receivedRequest);
        //            connectedUserRequests.Add(sentRequest);

        //            connectedUserRequestIds.Add(receivedRequest.Id);
        //            connectedUserRequestIds.Add(sentRequest.Id);
        //            break; // Connection requests matched, continue to next received request
        //        }
        //    }
        //}

        //// Sort the remaining unmatched sent requests
        //foreach (var sentRequest in userConnectionRequests_Sent)
        //{
        //    if (!connectedUserRequestIds.Contains(sentRequest.Id))
        //    { pendingUserRequests.Add(sentRequest); }
        //}

        //// Sort the remaining unmatched received requests
        //foreach (var receivedRequest in userConnectionRequests_Received)
        //{
        //    if (!connectedUserRequestIds.Contains(receivedRequest.Id))
        //    { awaitingUserRequests.Add(receivedRequest); }
        //}
        //#endregion

        //#region Extract and map user data from user connection requests
        //var connnectedUsers = new List<AppUserVM>();
        //var pendingUsers = new List<AppUserVM>();
        //var awaitingUsers = new List<AppUserVM>();

        //// connectedUsers: Create a filtered List of all unique user ids (who aren't the current user)
        //var uniqueUserIds = new List<string>();
        //foreach (var request in connectedUserRequests)
        //{

        //    if (request.SendingUserId == currentUserId && !uniqueUserIds.Contains(request.ReceivingUserId)) // If sent request
        //    {
        //        uniqueUserIds.Add(request.ReceivingUserId);
        //    }
        //    else if (request.ReceivingUserId == currentUserId && !uniqueUserIds.Contains(request.SendingUserId)) // If received request
        //    {
        //        uniqueUserIds.Add(request.SendingUserId);
        //    }
        //}

        //foreach (var userId in uniqueUserIds)
        //{
        //    connnectedUsers.Add(_mapper.Map<AppUserVM>(await _userManager.FindByIdAsync(userId)));
        //}
        //uniqueUserIds.Clear();

        //// pending
        //foreach (var request in pendingUserRequests)
        //{
        //    uniqueUserIds.Add(request.ReceivingUserId);
        //}

        //foreach (var userId in uniqueUserIds)
        //{
        //    pendingUsers.Add(_mapper.Map<AppUserVM>(await _userManager.FindByIdAsync(userId)));
        //}
        //uniqueUserIds.Clear();

        //// awaiting
        //foreach (var request in awaitingUserRequests)
        //{
        //    uniqueUserIds.Add(request.ReceivingUserId);
        //}

        //foreach (var userId in uniqueUserIds)
        //{
        //    awaitingUsers.Add(_mapper.Map<AppUserVM>(await _userManager.FindByIdAsync(userId)));
        //}
        //uniqueUserIds.Clear();
        #endregion

        #region Sort user connection requests 02
        // Get all UserConnectionRequest objs relevant to current user (current user either being a sender or receiver)
        var userConnectionRequests = _unitOfWork.UserConnectionRequests
            .GetSome(u => u.SendingUserId == currentUserId || u.ReceivingUserId == currentUserId);

        // Get all UserConnectionRequest_User objs related to the acquired UserConnectionRequests (foreign key)
        var userConnectionRequest_Users = new List<UserConnectionRequest_User>();
        foreach (var ucr in userConnectionRequests)
        {
            var ucr_User = await _unitOfWork.UserConnectionRequest_Users
                .GetOneAsync(u => u.UserConnectionRequestId == ucr.Id);

            if (ucr_User == null) continue;

            userConnectionRequest_Users.Add(ucr_User);
        }

        var connnectedUsers = new List<AppUserVM>();
        var pendingUsers = new List<AppUserVM>();
        var awaitingUsers = new List<AppUserVM>();

        // Connected users (2 matching UserConnectionRequest_User)
        foreach (var ucr in userConnectionRequests)
        {
            // For each UCR, check if both sending user and receiving UCR_User (id) exists
            var ucr_Users = userConnectionRequest_Users.FindAll(u => u.UserConnectionRequestId == ucr.Id); // Get all related UCR_User objs
            foreach (var ucr_User in ucr_Users)
            {
                // Checking if pending or connected
                if (ucr.SendingUserId == currentUserId && ucr_User.UserConnectionRequestId == ucr.Id)
                {
                    if (ucr_User.UserId == ucr.SendingUserId) continue; // Skip this UCR_User if contains current user's Id

                    // Check if receiving user's id exists in a related UCR_User
                    var receivingUser = userConnectionRequest_Users.Find(u => u.UserId == ucr.ReceivingUserId);
                    if (receivingUser == null)
                    {
                        // Receiving user has still yet to make a response (pending)
                        pendingUsers.Add(_mapper.Map<AppUserVM>(await _userManager.FindByIdAsync(ucr.ReceivingUserId)));
                    }
                    // Receiving user is connected with current user
                    connnectedUsers.Add(_mapper.Map<AppUserVM>(await _userManager.FindByIdAsync(ucr.ReceivingUserId)));
                }

                // Checking if awaiting or connected
                if (ucr.ReceivingUserId == currentUserId && ucr_User.UserId == ucr.ReceivingUserId)
                {

                }
            }

            // Map AppUser as AppUserVM
        }
        // Pending users (Users who received request from current user)

        // Awaiting users (Users who sent request to current user)
        #endregion

        return View(new ViewUsersVM
        {
            CurrentUserId = currentUserId,
            Users = users,
            //ConnectedUsers = connnectedUsers,
            //PendingUsers = pendingUsers,
            //AwaitingUsers = awaitingUsers
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

        #region DEMO: Demo admin cannot make user changes
        var currentUserEmail = _httpContextAccessor.HttpContext?.User.GetEmail();
        if (currentUserEmail.ToUpper() == "admin@example.com".ToUpper())
        {
            TempData["error"] = "Demo Admin not allowed to edit user data";
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

    // (Edit Roles only)
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

        //#region Update user roles based on form
        //var currentRoles = await _userManager.GetRolesAsync(user);
        //var inputRoles = form
        //foreach (var item in collection)
        //{

        //}
        //if (currentRoles.Contains)
        //{

        //}
        //#endregion

        var updateResult = await _userManager.UpdateAsync(user);
        if (!updateResult.Succeeded)
        {
            TempData["error"] = "Server error";
            return RedirectToAction("Index", "Home"); // (change to Previous url)
        }


        return RedirectToAction("Index", "Home"); // (change to Previous url)
    }

    [HttpGet]
    public async Task<IActionResult> SendUserConnectionRequest_ViewUsers(string id)
    {
        // Check if userId exists in db
        if (await _userManager.FindByIdAsync(id) == null)
        {
            TempData["error"] = "User doesn't exist";
            return RedirectToAction("ViewUsers", "User"); // (change to Previous url)
        }

        var currentUserId = _httpContextAccessor.HttpContext?.User.GetUserId(); // Get the current user id

        // Check if a connection request already exists (same sender and receiver id)

        // Create connection request object
        var userConnectionRequest = new UserConnectionRequest()
        {
            Id = 0,
            SendingUserId = currentUserId,
            ReceivingUserId = id,
            IsConnected = false
        };

        await _unitOfWork.UserConnectionRequests.AddAsync(userConnectionRequest);
        if (!await _unitOfWork.SaveAsync())
        {
            TempData["error"] = "Server error";
            return RedirectToAction("Index", "Home"); // (change to Previous url)
        }
        else
        {
            // Retrieve record data from db (containing the newly generated Id)
            userConnectionRequest = await _unitOfWork.UserConnectionRequests
                .GetOneAsync(u => u.SendingUserId == currentUserId && u.ReceivingUserId == id);
        }

        // Make sure the Id has been generated by the database
        if (userConnectionRequest.Id == 0)
        {
            TempData["error"] = "UserConnectionRequest object Id cannot be 0";
            return RedirectToAction("Index", "Home"); // (change to Previous url)
        }

        // Create ..._User object using sender's id
        var userConnectionRequest_User = new UserConnectionRequest_User()
        {
            UserId = currentUserId,
            UserConnectionRequestId = userConnectionRequest.Id
        };

        await _unitOfWork.UserConnectionRequest_Users.AddAsync(userConnectionRequest_User);
        if (!await _unitOfWork.SaveAsync())
        {
            TempData["error"] = "Server error";
            return RedirectToAction("Index", "Home"); // (change to Previous url)
        }

        return RedirectToAction("ViewUsers", "User");
    }
}

