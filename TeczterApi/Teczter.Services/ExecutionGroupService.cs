using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Teczter.Domain;
using Teczter.Domain.Entities;
using Teczter.Persistence;
using Teczter.Services.AdapterInterfaces;
using Teczter.Services.RequestDtos.ExecutionGroups;
using Teczter.Services.RequestDtos.Executions;
using Teczter.Services.ServiceInterfaces;

namespace Teczter.Services;

public class ExecutionGroupService(
        IExecutionGroupAdapter executionGroupAdapter,
        IExecutionGroupComposer composer,
        IValidator<ExecutionGroupEntity> validator,
        IUnitOfWork uow) : IExecutionGroupService
{
    private const int PageSize = 15;

    public async Task<TeczterValidationResult<ExecutionGroupEntity>> CloneExecutionGroup(
        ExecutionGroupEntity executionGroupToClone, 
        string newExecutionGroupName, string? 
        softwareVersionNumber,
        CancellationToken ct)
    {
        var newExecutionGroup = executionGroupToClone.CloneExecutionGroup(newExecutionGroupName, softwareVersionNumber);

        await executionGroupAdapter.AddNewExecutionGroup(newExecutionGroup, ct);

        var result = await ValidateExecutionGroup(newExecutionGroup, ct);

        if (result.IsValid)
        {
            await uow.SaveChanges(ct);
        }

        return result;
    }

    public async Task<TeczterValidationResult<ExecutionGroupEntity>> CreateExecution(ExecutionGroupEntity executionGroup, CreateExecutionRequestDto request, CancellationToken ct)
    {
        var groupResult = composer
            .UsingContext(executionGroup)
            .AddExecution(request)
            .Build();

        if (groupResult.IsValid)
        {
            var result = await ValidateExecutionGroup(groupResult.Value!, ct);

            if (result.IsValid)
            {
                await uow.SaveChanges(ct);
            }

            return result;
        }

        return groupResult;
    }

    public async Task<TeczterValidationResult<ExecutionGroupEntity>> CreateNewExecutionGroup(CreateExecutionGroupRequestDto request, CancellationToken ct)
    {
        var groupResult = composer
            .SetName(request.ExecutionGroupName)
            .SetSoftwareVersionNumber(request.SoftwareVersionNumber)
            .SetExecutionGroupNotes(request.ExecutionGroupNotes)
            .AddExecutions(request.Executions)
            .Build();

        if (!groupResult.IsValid)
        {
            return groupResult;
        }
        
        var result = await ValidateExecutionGroup(groupResult.Value!, ct);

        if (!result.IsValid)
        {
            return result;
        }
        
        await executionGroupAdapter.AddNewExecutionGroup(groupResult.Value!, ct);
        await uow.SaveChanges(ct);

        return result;

    }

    public async Task<TeczterValidationResult<ExecutionGroupEntity>> DeleteExecutionGroup(ExecutionGroupEntity executionGroup, CancellationToken ct)
    {
        var result = executionGroup.Delete();

        if (result.IsValid)
        {
            await uow.SaveChanges(ct);
        }

        return result;
    }

    public Task<ExecutionGroupEntity?> GetExecutionGroupById(int id, CancellationToken ct)
    {
        return executionGroupAdapter.GetExecutionGroupById(id, ct);
    }

    public async Task<List<ExecutionGroupEntity>> GetExecutionGroupSearchResults(int pageNumber, string? executionGroupName, string? versionNumber, CancellationToken ct)
    {
        var executionGroupQuery = executionGroupAdapter.GetBasicExecutionGroupSearchQuery();

        if (executionGroupName != null)
        {
            executionGroupQuery = executionGroupQuery.Where(x => x.ExecutionGroupName.Contains(executionGroupName));
        }

        if (versionNumber != null)
        {
            executionGroupQuery = executionGroupQuery.Where(x => x.SoftwareVersionNumber == versionNumber);
        }

        executionGroupQuery = executionGroupQuery.OrderByDescending(x => x.SoftwareVersionNumber);
        executionGroupQuery = executionGroupQuery.Skip((pageNumber - 1) * PageSize).Take(PageSize);

        return await executionGroupQuery.ToListAsync(ct);
    }

    public async Task<TeczterValidationResult<ExecutionGroupEntity>> ValidateExecutionGroup(ExecutionGroupEntity executionGroup, CancellationToken ct)
    {
        var result = await validator.ValidateAsync(executionGroup, ct);

        if (!result.IsValid)
        {
            return TeczterValidationResult<ExecutionGroupEntity>.Fail(result.Errors.Select(x => x.ErrorMessage).ToArray());
        }

        return TeczterValidationResult<ExecutionGroupEntity>.Succeed(executionGroup);
    }

    public async Task<TeczterValidationResult<ExecutionEntity>> DeleteExecution(ExecutionGroupEntity executionGroup, int executionId, CancellationToken ct)
    {
        var execution = executionGroup.Executions.Single(x => x.Id == executionId && !x.IsDeleted);

        var result = executionGroup.DeleteExecution(execution);

        if (result.IsValid)
        {
            await uow.SaveChanges(ct); 
        }

        return result;
    }
}
