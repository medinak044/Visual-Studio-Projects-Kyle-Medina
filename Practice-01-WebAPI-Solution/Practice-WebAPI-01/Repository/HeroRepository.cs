using Microsoft.EntityFrameworkCore;
using Practice_WebAPI_01.Data;
using Practice_WebAPI_01.Interfaces;
using Practice_WebAPI_01.Models;

namespace Practice_WebAPI_01.Repository;

public class HeroRepository: Repository<Hero>, IHeroRepository
{
    private readonly DataContext _context;

    public HeroRepository(DataContext context): base(context)
    {
        _context = context;
    }

    //public async Task<Hero> GetHero(int heroId)
    //{
    //    return await _context.Heroes.FirstOrDefaultAsync(h => h.Id == heroId);
    //}

    //public async Task<IEnumerable<Hero>> GetHeroes()
    //{
    //    return await _context.Heroes.ToListAsync();
    //}

    //public async Task<bool> HeroExists(string heroName)
    //{
    //    var x = await _context.Heroes.Where(h => h.UserName.Trim().ToUpper() == heroName.Trim().ToUpper()).FirstOrDefaultAsync();

    //    if (x != null) 
    //        return true; // If Hero name already exists

    //    return false;
    //}

    //public async Task<bool> RegisterHero(Hero hero)
    //{
    //    await _context.AddAsync(hero);
    //    return await Save();
    //}

    //public async Task<bool> DeleteHero (Hero hero)
    //{
    //    _context.Remove(hero);
    //    return await Save();
    //}

    //public async Task<bool> Save()
    //{
    //    var saved = await _context.SaveChangesAsync();
    //    return saved > 0 ? true : false;
    //}
}
