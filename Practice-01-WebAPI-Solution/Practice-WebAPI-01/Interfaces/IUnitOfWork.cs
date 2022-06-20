using Practice_WebAPI_01.Interfaces;

namespace Practice_WebAPI_01.Interfaces;

public interface IUnitOfWork // "IAsyncDisposable" is available as well
{
    //IHeroRepository Hero { get; }
    //IWeaponRepository Weapon { get; }
    IWeaponTypeRepository WeaponTypes { get; }
    Task<bool> Save();
}
