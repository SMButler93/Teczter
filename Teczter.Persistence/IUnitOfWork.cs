namespace Teczter.Persistence;

public interface IUnitOfWork
{
    Task CommitChanges();
    void Rollback();
}
