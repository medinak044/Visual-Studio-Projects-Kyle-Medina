using Practice_WebAPI_01.Data;
using Practice_WebAPI_01.Interfaces;
using Practice_WebAPI_01.Models;
using System.Collections;

namespace Practice_WebAPI_01.Repository;


public class UnitOfWork : IUnitOfWork, IDisposable
{
    private readonly DataContext _context;

    public IHeroRepository Heroes { get; private set; }
    public IWeaponRepository Weapons { get; private set; }
    public IWeaponTypeRepository WeaponTypes { get; private set; }

    public UnitOfWork(DataContext context)
    {
        _context = context;
        Heroes = new HeroRepository(_context);
        Weapons = new WeaponRepository(_context);
        WeaponTypes = new WeaponTypeRepository(_context);

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

    //public async Task<bool> Dispose()
    //{
    //    await _context.DisposeAsync();
    //    return true;
    //}
}

//public class UnitOfWork : IUnitOfWork
//{
//    private readonly DataContext _context;
//    private Hashtable _repositories;

//    public UnitOfWork(DataContext context)
//    {
//        _context = context;
//    }


//    public async Task<bool> Save()
//    {
//        var saved = await _context.SaveChangesAsync();
//        return saved > 0 ? true : false;
//    }

//    public void Dispose()
//    {
//        _context.Dispose();
//    }

//    // 7:33 https://www.udemy.com/course/learn-to-build-an-e-commerce-app-with-net-core-and-angular/learn/lecture/18138230#overview
//    public IRepository<TEntity> Repository<TEntity>() where TEntity : BaseEntity
//    {
//        // Check if a hashtable currently exists or not
//        //if (_repositories == null) _repositories = new Hashtable();
//        _repositories ??= new Hashtable();

//        var type = typeof(TEntity).Name; // Get the name of the type provided

//        // Check if the hashtable contains a key of the same type name
//        if (!_repositories.ContainsKey(type))
//        {
//            var repositoryType = typeof(Repository<>);
//            var repositoryInstance = Activator.CreateInstance(repositoryType
//                .MakeGenericType(typeof(TEntity)), _context);

//            _repositories.Add(type, repositoryInstance);
//        }

//        return (IRepository<TEntity>)_repositories[type];
//    }
//}
