namespace Oversee.Repository.IRepository;

public interface IUnitOfWork
{
    Task<bool> SaveAsync();
    IItemRepository Items { get; }
    IItemRecordRepository ItemRecords { get; }
    IItemRecordUserRepository ItemRecordUsers { get; }
    IItemRequestRepository ItemRequests { get; }
    IUserConnectionRequestRepository UserConnectionRequests { get; }
}
