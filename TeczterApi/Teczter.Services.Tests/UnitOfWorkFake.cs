using Teczter.Persistence;

namespace Teczter.Services.Tests;

public class UnitOfWorkFake : IUnitOfWork
{
    public Task SaveChanges(CancellationToken ct)
    {
        return Task.CompletedTask;
    }
}