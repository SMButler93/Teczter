using Teczter.Persistence;

namespace Teczter.Services.Tests;

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