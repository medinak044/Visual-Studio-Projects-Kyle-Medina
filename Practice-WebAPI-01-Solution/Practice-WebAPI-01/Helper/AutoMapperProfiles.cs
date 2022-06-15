using AutoMapper;
using Practice_WebAPI_01.DTOs;
using Practice_WebAPI_01.Models;

namespace Practice_WebAPI_01.Helper
{
    // ref: https://youtu.be/K4WuxwkXrIY?list=PL82C6-O4XrHdiS10BLh23x71ve9mQCln0&t=1587
    public class AutoMapperProfiles: Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<Hero, HeroDto>();
            CreateMap<HeroDto, Hero>(); // "RegisterHero()"
        }
    }
}
