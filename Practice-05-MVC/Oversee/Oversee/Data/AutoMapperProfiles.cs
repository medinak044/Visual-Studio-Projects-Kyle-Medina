using AutoMapper;
using Oversee.Models;
using Oversee.ViewModels;

namespace Oversee.Data;

public class AutoMapperProfiles: Profile
{
    public AutoMapperProfiles()
    {
        CreateMap<RegisterVM, AppUser>(); // When registering new user
    }
}
