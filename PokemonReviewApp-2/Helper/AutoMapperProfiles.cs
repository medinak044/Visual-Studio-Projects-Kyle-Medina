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
        }
    }
}
