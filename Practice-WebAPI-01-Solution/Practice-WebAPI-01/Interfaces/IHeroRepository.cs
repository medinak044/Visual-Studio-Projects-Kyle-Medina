using Practice_WebAPI_01.Models;

namespace Practice_WebAPI_01.Interfaces
{
    public interface IHeroRepository
    {
        Task<Hero> GetHero(int heroId);
        Task <ICollection<Hero>> GetHeroes();
        Task<bool> HeroExists(string heroName);
        Task<bool> RegisterHero(Hero hero);
        Task<bool> Save();
    }
}
