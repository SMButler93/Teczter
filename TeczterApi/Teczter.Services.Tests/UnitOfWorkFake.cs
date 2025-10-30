using Teczter.Persistence;

namespace Teczter.Services.Tests;

public class UnitOfWorkFake : IUnitOfWork
{
    public Task SaveChanges()
    {
        return Task.CompletedTask;
    }

    public void Rollback()
    {
    }
}