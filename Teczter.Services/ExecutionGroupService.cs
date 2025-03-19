using Microsoft.EntityFrameworkCore;
using Teczter.Adapters.AdapterInterfaces;
using Teczter.Domain.Entities;
using Teczter.Persistence;
using Teczter.Services.RequestDtos.Request;
using Teczter.Services.ServiceInterfaces;

namespace Teczter.Services;

public class ExecutionGroupService : IExecutionGroupService
{
    private readonly IExecutionGroupAdapter _executionGroupAdapter;
    private readonly IExecutionGroupBuilder _builder;
    private readonly IUnitOfWork _uow;

    public ExecutionGroupService(
        IExecutionGroupAdapter executionGroupAdapter, 
        IExecutionGroupBuilder builder,
        IUnitOfWork uow)
    {
        _executionGroupAdapter = executionGroupAdapter;
        _builder = builder;
        _uow = uow;
    }

    public async Task<ExecutionGroupEntity> CloneExecutionGroup(ExecutionGroupEntity executiongroupToClone, string newExecutionGroupName, string? softwareVersionNumber)
    {
        var newExecutionGroup = executiongroupToClone.CloneExecutionGroup(newExecutionGroupName, softwareVersionNumber);

        await _executionGroupAdapter.CreateNewExecutionGroup(newExecutionGroup);

        newExecutionGroup.Executions = executiongroupToClone.Executions.Select(x => x.CloneExecution(newExecutionGroup.Id)).ToList();

        await _uow.CommitChanges();

        return newExecutionGroup;
    }

    public async Task<ExecutionGroupEntity> CreateNewExecutionGroup(CreateExecutionGroupRequestDto request)
    {
        var executionGroup = _builder
            .NewInstance()
            .SetName(request.ExecutionGroupName)
            .SetSoftwareVersionNumber(request.SoftwareVersionNumber)
            .SetExecutionGroupNotes(request.ExecutionGroupNotes)
            .AddExecutions(request.Executions)
            .Build();

        await _executionGroupAdapter.CreateNewExecutionGroup(executionGroup);

        return executionGroup;
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
