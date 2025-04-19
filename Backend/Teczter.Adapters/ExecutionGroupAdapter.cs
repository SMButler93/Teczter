using Microsoft.EntityFrameworkCore;
using Teczter.Data;
using Teczter.Domain.Entities;
using Teczter.Services.AdapterInterfaces;

namespace Teczter.Adapters;

public class ExecutionGroupAdapter(TeczterDbContext dbContext) : IExecutionGroupAdapter
{
    private readonly TeczterDbContext _dbContext = dbContext;

    public async Task CreateNewExecutionGroup(ExecutionGroupEntity executionGroup)
    {
        await _dbContext.ExecutionGroups.AddAsync(executionGroup);
    }

    public IQueryable<ExecutionGroupEntity> GetBasicExecutionGroupSearchQuery()
    {
        return _dbContext.ExecutionGroups
            .Include(x => x.Executions)
            .Where(x => !x.IsDeleted);
    }

    public async Task<ExecutionGroupEntity?> GetExecutionGroupById(int id)
    {
        return await _dbContext.ExecutionGroups
            .Include(x => x.Executions)
            .SingleOrDefaultAsync(x => x.Id == id && !x.IsDeleted);
    }
}