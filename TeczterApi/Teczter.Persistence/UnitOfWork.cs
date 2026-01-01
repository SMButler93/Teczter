using Teczter.Data;

namespace Teczter.Persistence;

public class UnitOfWork(TeczterDbContext dbContext) : IUnitOfWork
{
    public async Task SaveChanges(CancellationToken ct)
    {
        await dbContext.SaveChangesAsync(ct);
    }
}
