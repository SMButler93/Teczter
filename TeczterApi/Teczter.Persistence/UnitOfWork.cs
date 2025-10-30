using Teczter.Data;

namespace Teczter.Persistence;

public class UnitOfWork : IUnitOfWork
{
    private readonly TeczterDbContext _dbContext;

    public UnitOfWork(TeczterDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task SaveChanges()
    {
        await _dbContext.SaveChangesAsync();
    }

    public void Rollback()
    {
        _dbContext.ChangeTracker.Clear();
    }
}
