using Microsoft.EntityFrameworkCore;
using Teczter.Data;
using Teczter.Domain.Entities;
using Teczter.Services.AdapterInterfaces;

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
