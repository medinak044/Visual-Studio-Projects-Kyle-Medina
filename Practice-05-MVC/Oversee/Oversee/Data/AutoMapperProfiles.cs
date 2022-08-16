using AutoMapper;
using Oversee.Models;
using Oversee.ViewModels;

namespace Oversee.Data;

public class AutoMapperProfiles: Profile
{
    public AutoMapperProfiles()
    {
        CreateMap<RegisterVM, AppUser>(); // When registering new user
        CreateMap<AppUser, AppUserVM>(); // When displaying user details
        CreateMap<AppUser, AppUser_AdminVM>(); // When displaying table of users in admin view

        CreateMap<AppUser, ProfileEditVM>(); // Edit user form
        CreateMap<ProfileEditVM, AppUser>(); // Submit edits to user
        CreateMap<AppUser, ProfileEdit_AdminVM>(); // (Admin) Edit user form
        CreateMap<ProfileEdit_AdminVM, AppUser>(); // (Admin) Submit edits to user
    }
}
