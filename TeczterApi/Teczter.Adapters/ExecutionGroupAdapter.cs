using Microsoft.EntityFrameworkCore;
using Teczter.Data;
using Teczter.Domain.Entities;
using Teczter.Services.AdapterInterfaces;

namespace Teczter.Adapters;

public class ExecutionGroupAdapter(TeczterDbContext _dbContext) : IExecutionGroupAdapter
{
    private const int PageSize = 15;
    public async Task AddNewExecutionGroup(ExecutionGroupEntity executionGroup, CancellationToken ct)
    {
        await _dbContext.ExecutionGroups.AddAsync(executionGroup, ct);
    }

    public async Task<List<ExecutionGroupEntity>> GetExecutionGroupSearchResults(int pageNumber, string? executionGroupName, string? versionNumber, CancellationToken ct)
    {
        var query = _dbContext.ExecutionGroups
            .AsNoTracking()
            .Include(x => x.Executions)
            .Where(x => !x.IsDeleted);
        
        if (executionGroupName != null)
        {
            query = query.Where(x => EF.Functions.ILike(x.ExecutionGroupName, $"%{executionGroupName}%"));
        }

        if (versionNumber != null)
        {
            query = query.Where(x => x.SoftwareVersionNumber != null && EF.Functions.ILike(x.SoftwareVersionNumber, $"%{versionNumber}%"));
        }

        query = query.OrderByDescending(x => x.SoftwareVersionNumber);
        query = query.Skip((pageNumber - 1) * PageSize).Take(PageSize);

        return await query.ToListAsync(ct);
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