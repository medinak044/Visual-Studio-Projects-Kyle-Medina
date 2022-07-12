using Practice_WebAPI_01.Interfaces;
using Practice_WebAPI_01.Models;

namespace Practice_WebAPI_01.Interfaces;

public interface IUnitOfWork : IDisposable // "IAsyncDisposable" is available as well
{
    IHeroRepository Heroes { get; }
    IWeaponRepository Weapons { get; }
    IWeaponTypeRepository WeaponTypes { get; }
    Task<bool> Save();
    //IRepository<TEntity> Repository<TEntity>() where TEntity : BaseEntity;
}
