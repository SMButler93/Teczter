using Teczter.Data;
using Teczter.Data.MiddlewareModels;

namespace Teczter.Adapters.MiddlewareRepositories.RequestLogRepository;

public class RequestLogRepository(TeczterDbContext dbContext) : IRequestLogRepository
{
    private readonly TeczterDbContext _dbContext = dbContext;

    public async Task LogRequest(RequestLog log)
    {
        await _dbContext.RequestLogs.AddAsync(log);
        await _dbContext.SaveChangesAsync();
    }
}