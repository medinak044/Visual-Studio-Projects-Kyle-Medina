using AutoMapper;
using PokemonReviewApp_2.Dto;
using PokemonReviewApp_2.Models;

namespace PokemonReviewApp_2.Helper
{
    public class AutoMapperProfiles: Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<Pokemon, PokemonDto>();
            CreateMap<PokemonDto, Pokemon>(); //
            CreateMap<Category, CategoryDto>();
            CreateMap<CategoryDto, Category>(); // For the "CreateCategory" post request
            CreateMap<Country, CountryDto>();
            CreateMap<CountryDto, Country>(); // For the "CreateCountry" post request
            CreateMap<Owner, OwnerDto>();
            CreateMap<OwnerDto, Owner>();  // For the "CreateOwner" post request
            CreateMap<Review, ReviewDto>();
            CreateMap<ReviewDto, Review>(); //
            CreateMap<Reviewer, ReviewerDto>();
            CreateMap<ReviewerDto, Reviewer>(); //
        }
    }
}
