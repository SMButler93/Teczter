using Microsoft.EntityFrameworkCore;
using Teczter.Data;
using Teczter.Domain.Entities;
using Teczter.Services.AdapterInterfaces;

namespace Teczter.Adapters;

public class ExecutionAdapter(TeczterDbContext _dbContext) : IExecutionAdapter
{
    public async Task<ExecutionEntity?> GetExecutionById(int id, CancellationToken ct)
    {
        return await _dbContext.Executions
            .Include(x => x.ExecutionGroup)
            .Include(x => x.Test)
            .SingleOrDefaultAsync(x => x.Id == id && !x.IsDeleted, ct);
    }

    public async Task<List<ExecutionEntity>> GetExecutionsForTest(int testId,  CancellationToken ct)
    {
        return await _dbContext.Executions
            .Where(x => x.TestId == testId && !x.IsDeleted)
            .ToListAsync(ct);
    }
}
