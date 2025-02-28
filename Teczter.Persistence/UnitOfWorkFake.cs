namespace Teczter.Persistence;

public class UnitOfWorkFake : IUnitOfWork
{
    public Task CommitChanges()
    {
        return Task.CompletedTask;
    }

    public void Rollback()
    {
    }
}