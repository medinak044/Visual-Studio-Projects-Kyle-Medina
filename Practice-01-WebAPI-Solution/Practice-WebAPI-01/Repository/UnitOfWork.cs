using Practice_WebAPI_01.Data;
using Practice_WebAPI_01.Interfaces;

namespace Practice_WebAPI_01.Repository;

public class UnitOfWork : IUnitOfWork, IDisposable
{
    private readonly DataContext _context;

    public IHeroRepository Heroes { get; private set; }
    //public IWeaponRepository Weapons { get; private set; }
    public IWeaponTypeRepository WeaponTypes { get; private set; }

    public UnitOfWork(DataContext context)
    {
        _context = context;
        Heroes = new HeroRepository(_context);
        //Weapons = new WeaponRepository(_context);
        WeaponTypes = new WeaponTypeRepository(_context);

    }


    public async Task<bool> Save()
    {
        var saved = await _context.SaveChangesAsync();
        return saved > 0 ? true : false;
    }

    //public async Task<bool> Dispose()
    //{
    //    await _context.DisposeAsync();
    //    return true;
    //}

    public void Dispose()
    {
        _context.DisposeAsync();
    }
}
