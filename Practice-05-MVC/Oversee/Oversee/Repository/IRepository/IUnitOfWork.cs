namespace Oversee.Repository.IRepository;

public interface IUnitOfWork
{
    Task<bool> SaveAsync();
    //IItemRepository Items { get; }
}
