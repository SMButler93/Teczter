using Microsoft.EntityFrameworkCore;
using Teczter.Adapters.AdapterInterfaces;
using Teczter.Domain.Entities;
using Teczter.Services.ServiceInterfaces;

namespace Teczter.Services;

public class ExecutionGroupService : IExecutionGroupService
{
    private readonly IExecutionGroupAdapter _executionGroupAdapter;

    public ExecutionGroupService(IExecutionGroupAdapter executionGroupAdapter)
    {
        _executionGroupAdapter = executionGroupAdapter;
    }

    public async Task<List<ExecutionGroupEntity>> GetExecutionGroupSearchResults(string? executionGroupName, string? versionNumber)
    {
        var executionGroupQuery = _executionGroupAdapter.GetBasicExecutionGroupSearchQuery();

        executionGroupQuery = executionGroupName == null ? executionGroupQuery : executionGroupQuery.Where(x => x.ExecutionGroupName.Contains(executionGroupName));
        executionGroupQuery = versionNumber == null ? executionGroupQuery : executionGroupQuery.Where(x => x.SoftwareVersionNumber == versionNumber);

        return await executionGroupQuery.ToListAsync();
    }
}
