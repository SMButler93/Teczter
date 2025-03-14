using Microsoft.EntityFrameworkCore;
using Teczter.Adapters.AdapterInterfaces;
using Teczter.Data;
using Teczter.Domain.Entities;

namespace Teczter.Adapters;

public class ExecutionGroupAdapter : IExecutionGroupAdapter
{
    private readonly TeczterDbContext _dbContext;

    public ExecutionGroupAdapter(TeczterDbContext dbContext)
    {
        _dbContext = dbContext;
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