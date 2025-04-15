using Microsoft.EntityFrameworkCore;
using Teczter.Data;
using Teczter.Domain.Entities;
using Teczter.Services.AdapterInterfaces;

namespace Teczter.Adapters;

public class ExecutionAdapter(TeczterDbContext dbContext) : IExecutionAdapter
{
    private readonly TeczterDbContext _dbContext = dbContext;

    public async Task<List<ExecutionEntity>> GetExecutionsForTest(int testId)
    {
        return await _dbContext.Executions.Where(x => x.TestId == testId && !x.IsDeleted).ToListAsync();
    }
}
