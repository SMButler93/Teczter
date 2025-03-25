using Microsoft.EntityFrameworkCore;
using Teczter.Adapters.AdapterInterfaces;
using Teczter.Data;
using Teczter.Domain.Entities;

namespace Teczter.Adapters;

public class ExecutionAdapter : IExecutionAdapter
{
    private readonly TeczterDbContext _dbContext;

    public ExecutionAdapter(TeczterDbContext dbCobtext)
    {
        _dbContext = dbCobtext;
    }

    public async Task<List<ExecutionEntity>> GetExecutionsForTest(int testId)
    {
        return await _dbContext.Executions.Where(x => x.TestId == testId && !x.IsDeleted).ToListAsync();
    }
}
