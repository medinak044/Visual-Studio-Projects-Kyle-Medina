using Microsoft.EntityFrameworkCore;
using Practice_WebAPI_01.Data;
using Practice_WebAPI_01.Interfaces;
using Practice_WebAPI_01.Models;

namespace Practice_WebAPI_01.Repository
{
    public class HeroRepository: IHeroRepository
    {
        private readonly DataContext _context;

        public HeroRepository(DataContext context)
        {
            _context = context;
        }

        public async Task<Hero> GetHero(int heroId)
        {
            return await _context.Heroes.FirstOrDefaultAsync(h => h.Id == heroId);
        }

        public async Task<ICollection<Hero>> GetHeroes()
        {
            return await _context.Heroes.ToListAsync();
        }

        public async Task<bool> HeroExists(string heroName)
        {
            var x = await _context.Heroes.Where(h => h.UserName.Trim().ToUpper() == heroName.Trim().ToUpper()).FirstOrDefaultAsync();

            if (x != null) 
                return true; // If Hero name already exists

            return false;
        }
    }
}
