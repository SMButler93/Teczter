using Microsoft.EntityFrameworkCore;
using Teczter.Data;
using Teczter.Domain.Entities;
using Teczter.Services.AdapterInterfaces;

namespace Teczter.Adapters;

public class ExecutionGroupAdapter(TeczterDbContext dbContext) : IExecutionGroupAdapter
{
    private readonly TeczterDbContext _dbContext = dbContext;

    public async Task AddNewExecutionGroup(ExecutionGroupEntity executionGroup, CancellationToken ct)
    {
        await _dbContext.ExecutionGroups.AddAsync(executionGroup, ct);
    }

    public IQueryable<ExecutionGroupEntity> GetBasicExecutionGroupSearchQuery()
    {
        return _dbContext.ExecutionGroups
            .AsNoTracking()
            .Include(x => x.Executions)
            .Where(x => !x.IsDeleted);
    }

    public async Task<ExecutionGroupEntity?> GetExecutionGroupById(int id, CancellationToken ct)
    {
        return await _dbContext.ExecutionGroups
            .Include(x => x.Executions)
            .ThenInclude(y => y.Test)
            .ThenInclude(z => z.TestSteps)
            .SingleOrDefaultAsync(x => x.Id == id && !x.IsDeleted, ct);
    }
}