using Microsoft.EntityFrameworkCore;
using Teczter.Data;
using Teczter.Domain.Entities;
using Teczter.Services.AdapterInterfaces;

namespace Teczter.Adapters;

public class ExecutionAdapter(TeczterDbContext dbContext) : IExecutionAdapter
{
    private readonly TeczterDbContext _dbContext = dbContext;

    public async Task<ExecutionEntity?> GetExecutionById(int id)
    {
        return await _dbContext.Executions
            .AsNoTracking()
            .SingleOrDefaultAsync(x => x.Id == id && !x.IsDeleted);
    }

    public async Task<List<ExecutionEntity>> GetExecutionsForTest(int testId)
    {
        return await _dbContext.Executions.Where(x => x.TestId == testId && !x.IsDeleted).ToListAsync();
    }
}
