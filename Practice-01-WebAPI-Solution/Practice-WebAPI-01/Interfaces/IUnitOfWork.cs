using Practice_WebAPI_01.Interfaces;

namespace Practice_WebAPI_01.Interfaces;

public interface IUnitOfWork: IDisposable // "IAsyncDisposable" is available as well
{
    //IHeroRepository Hero { get; }
    //IWeaponRepository Weapon { get; }
    IWeaponTypeRepository WeaponType { get; }
    Task<bool> Save();
}
