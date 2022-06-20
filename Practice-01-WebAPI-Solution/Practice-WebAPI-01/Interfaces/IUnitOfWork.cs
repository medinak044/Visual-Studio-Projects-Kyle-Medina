using Practice_WebAPI_01.Interfaces;

namespace Practice_WebAPI_01.Interfaces;

public interface IUnitOfWork // "IAsyncDisposable" is available as well
{
    IHeroRepository Heroes { get; }
    //IWeaponRepository Weapons { get; }
    IWeaponTypeRepository WeaponTypes { get; }
    Task<bool> Save();
}
