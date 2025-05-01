using Teczter.Data;
using Teczter.Data.MiddlewareModels;

namespace Teczter.Adapters.MiddlewareRepositories.ErrorLogRepository;

public class ErrorLogRepository(TeczterDbContext dbContext) : IErrorLogRepository
{
    private readonly TeczterDbContext _dbContext = dbContext;

    public async Task LogError(ErrorLog log)
    {
        await _dbContext.ErrorLogs.AddAsync(log);
        await _dbContext.SaveChangesAsync();
    }
}