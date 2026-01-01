namespace Teczter.Persistence;

public interface IUnitOfWork
{
    Task SaveChanges(CancellationToken ct);
}
