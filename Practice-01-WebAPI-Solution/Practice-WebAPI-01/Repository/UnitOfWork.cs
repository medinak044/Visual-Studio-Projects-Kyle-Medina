using Practice_WebAPI_01.Data;
using Practice_WebAPI_01.Interfaces;

namespace Practice_WebAPI_01.Repository;

public class UnitOfWork : IUnitOfWork
{
    private readonly DataContext _context;

    //public IHeroRepository Hero { get; private set; }
    //public IWeaponRepository Weapon { get; private set; }
    public IWeaponTypeRepository WeaponType { get; private set; }

    public UnitOfWork(DataContext context)
    {
        _context = context;
        //Hero = new HeroRepository(_context);
        //Weapon = new WeaponRepository(_context);
        WeaponType = new WeaponTypeRepository(_context);

    }


    public async Task<bool> Save()
    {
        var saved = await _context.SaveChangesAsync();
        return saved > 0 ? true : false;
    }

    public void Dispose()
    {
        _context.Dispose();
    }
}
