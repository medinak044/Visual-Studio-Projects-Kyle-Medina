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

        public async Task<ICollection<Hero>> GetHeroes()
        {
            return await _context.Heroes.ToListAsync();
        }
    }
}
