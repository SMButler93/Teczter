using Teczter.Data;

namespace Teczter.Persistence;

public class UnitOfWork(TeczterDbContext _dbContext) : IUnitOfWork
{
    public async Task SaveChanges(CancellationToken ct)
    {
        await _dbContext.SaveChangesAsync(ct);
    }
}
