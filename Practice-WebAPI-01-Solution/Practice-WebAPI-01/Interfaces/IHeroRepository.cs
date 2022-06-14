using Practice_WebAPI_01.Models;

namespace Practice_WebAPI_01.Interfaces
{
    public interface IHeroRepository
    {
        Task <ICollection<Hero>> GetHeroes();
    }
}
