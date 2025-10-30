namespace Teczter.Persistence;

public interface IUnitOfWork
{
    Task SaveChanges();
    void Rollback();
}
