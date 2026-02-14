using FluentValidation;
using Teczter.Domain;
using Teczter.Domain.Entities;
using Teczter.Persistence;
using Teczter.Services.AdapterInterfaces;
using Teczter.Services.RequestDtos.ExecutionGroups;
using Teczter.Services.RequestDtos.Executions;
using Teczter.Services.ServiceInterfaces;

namespace Teczter.Services;

public class ExecutionGroupService(
        IExecutionGroupAdapter _executionGroupAdapter,
        IExecutionGroupComposer _composer,
        IValidator<ExecutionGroupEntity> _validator,
        IUnitOfWork _uow) : IExecutionGroupService
{
    public async Task<TeczterValidationResult<ExecutionGroupEntity>> CloneExecutionGroup(
        ExecutionGroupEntity executionGroupToClone, 
        string newExecutionGroupName, string? 
        softwareVersionNumber,
        CancellationToken ct)
    {
        var newExecutionGroup = executionGroupToClone.CloneExecutionGroup(newExecutionGroupName, softwareVersionNumber);

        await _executionGroupAdapter.AddNewExecutionGroup(newExecutionGroup, ct);

        var result = await ValidateExecutionGroup(newExecutionGroup, ct);

        if (result.IsValid) await _uow.SaveChanges(ct);
        
        return result;
    }

    public async Task<TeczterValidationResult<ExecutionGroupEntity>> CreateExecution(int groupId, CreateExecutionRequestDto request, CancellationToken ct)
    {
        var executionGroup = await _executionGroupAdapter.GetExecutionGroupById(groupId, ct);

        if (executionGroup is null)
        {
            return TeczterValidationResult<ExecutionGroupEntity>.Fail($"No active execution group found with ID: {groupId}.");
        }
        
        _composer
            .UsingContext(executionGroup)
            .AddExecution(request)
            .Build();
        
        var result = await ValidateExecutionGroup(executionGroup, ct);

        if (result.IsValid) await _uow.SaveChanges(ct);

        return result;
    }

    public async Task<TeczterValidationResult<ExecutionGroupEntity>> CreateNewExecutionGroup(CreateExecutionGroupRequestDto request, CancellationToken ct)
    {
        var executionGroup = _composer
            .SetName(request.ExecutionGroupName)
            .SetSoftwareVersionNumber(request.SoftwareVersionNumber)
            .SetExecutionGroupNotes(request.ExecutionGroupNotes)
            .AddExecutions(request.Executions)
            .Build();
        
        var result = await ValidateExecutionGroup(executionGroup, ct);

        if (!result.IsValid) return result;
        
        await _executionGroupAdapter.AddNewExecutionGroup(executionGroup, ct);
        await _uow.SaveChanges(ct);

        return result;
    }

    public async Task<TeczterValidationResult> DeleteExecutionGroup(ExecutionGroupEntity executionGroup, CancellationToken ct)
    {
        if (executionGroup.IsClosed)
        {
            return TeczterValidationResult.Fail("Cannot delete an execution from an execution group that is closed.");
        }
        
        executionGroup.Delete();
        await _uow.SaveChanges(ct);

        return TeczterValidationResult.Succeed();
    }

    public Task<ExecutionGroupEntity?> GetExecutionGroupById(int id, CancellationToken ct)
    {
        return _executionGroupAdapter.GetExecutionGroupById(id, ct);
    }

    public async Task<List<ExecutionGroupEntity>> GetExecutionGroupSearchResults(int pageNumber, string? executionGroupName, string? versionNumber, CancellationToken ct)
    {
        return await _executionGroupAdapter.GetExecutionGroupSearchResults(pageNumber, executionGroupName, versionNumber, ct);
    }
    
    private async Task<TeczterValidationResult<ExecutionGroupEntity>> ValidateExecutionGroup(ExecutionGroupEntity executionGroup, CancellationToken ct)
    {
        var result = await _validator.ValidateAsync(executionGroup, ct);

        return result.IsValid 
            ? TeczterValidationResult<ExecutionGroupEntity>.Succeed(executionGroup) 
            : TeczterValidationResult<ExecutionGroupEntity>.Fail(result.Errors.Select(x => x.ErrorMessage).ToArray());
    }
}
