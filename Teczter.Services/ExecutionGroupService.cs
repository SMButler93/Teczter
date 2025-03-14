using Microsoft.EntityFrameworkCore;
using Teczter.Adapters.AdapterInterfaces;
using Teczter.Domain.Entities;
using Teczter.Persistence;
using Teczter.Services.ServiceInterfaces;

namespace Teczter.Services;

public class ExecutionGroupService : IExecutionGroupService
{
    private readonly IExecutionGroupAdapter _executionGroupAdapter;
    private readonly IUnitOfWork _uow;

    public ExecutionGroupService(IExecutionGroupAdapter executionGroupAdapter, IUnitOfWork uow)
    {
        _executionGroupAdapter = executionGroupAdapter;
        _uow = uow;
    }

    public async Task DeleteExecutionGroup(ExecutionGroupEntity executionGroup)
    {
        executionGroup.Delete();
        await _uow.CommitChanges();
    }

    public Task<ExecutionGroupEntity?> GetExecutionGroupById(int id)
    {
        return _executionGroupAdapter.GetExecutionGroupById(id);
    }

    public async Task<List<ExecutionGroupEntity>> GetExecutionGroupSearchResults(string? executionGroupName, string? versionNumber)
    {
        var executionGroupQuery = _executionGroupAdapter.GetBasicExecutionGroupSearchQuery();

        executionGroupQuery = executionGroupName == null ? executionGroupQuery : executionGroupQuery.Where(x => x.ExecutionGroupName.Contains(executionGroupName));
        executionGroupQuery = versionNumber == null ? executionGroupQuery : executionGroupQuery.Where(x => x.SoftwareVersionNumber == versionNumber);

        return await executionGroupQuery.ToListAsync();
    }
}
