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
        IExecutionGroupAdapter _executionGroupAdapter,
        IExecutionGroupComposer _composer,
        IValidator<ExecutionGroupEntity> _validator,
        IUnitOfWork _uow) : IExecutionGroupService
{
    private const int pageSize = 15;

    public async Task<TeczterValidationResult<ExecutionGroupEntity>> CloneExecutionGroup(
        ExecutionGroupEntity executionGroupToClone, 
        string newExecutionGroupName, string? 
        softwareVersionNumber)
    {
        var newExecutionGroup = executionGroupToClone.CloneExecutionGroup(newExecutionGroupName, softwareVersionNumber);

        await _executionGroupAdapter.AddNewExecutionGroup(newExecutionGroup);

        var result = await ValidateExecutionGroup(newExecutionGroup);

        if (result.IsValid)
        {
            await _uow.SaveChanges();
        }

        return result;
    }

    public async Task<TeczterValidationResult<ExecutionGroupEntity>> CreateExecution(ExecutionGroupEntity executionGroup, CreateExecutionRequestDto request)
    {
        var groupResult = _composer
            .UsingContext(executionGroup)
            .AddExecution(request)
            .Build();

        if (groupResult.IsValid)
        {
            var result = await ValidateExecutionGroup(groupResult.Value!);

            if (result.IsValid)
            {
                await _uow.SaveChanges();
            }

            return result;
        }

        return groupResult;
    }

    public async Task<TeczterValidationResult<ExecutionGroupEntity>> CreateNewExecutionGroup(CreateExecutionGroupRequestDto request)
    {
        var groupResult = _composer
            .SetName(request.ExecutionGroupName)
            .SetSoftwareVersionNumber(request.SoftwareVersionNumber)
            .SetExecutionGroupNotes(request.ExecutionGroupNotes)
            .AddExecutions(request.Executions)
            .Build();

        if (groupResult.IsValid)
        {
            var result = await ValidateExecutionGroup(groupResult.Value!);

            if (result.IsValid)
            {
                await _executionGroupAdapter.AddNewExecutionGroup(groupResult.Value!);
                await _uow.SaveChanges();
            }

            return result;
        }

        return groupResult;
    }

    public async Task<TeczterValidationResult<ExecutionGroupEntity>> DeleteExecutionGroup(ExecutionGroupEntity executionGroup)
    {
        var result = executionGroup.Delete();

        if (result.IsValid)
        {
            await _uow.SaveChanges();
        }

        return result;
    }

    public Task<ExecutionGroupEntity?> GetExecutionGroupById(int id)
    {
        return _executionGroupAdapter.GetExecutionGroupById(id);
    }

    public async Task<List<ExecutionGroupEntity>> GetExecutionGroupSearchResults(int pageNumber, string? executionGroupName, string? versionNumber)
    {
        var executionGroupQuery = _executionGroupAdapter.GetBasicExecutionGroupSearchQuery();

        if (executionGroupName != null)
        {
            executionGroupQuery = executionGroupQuery.Where(x => x.ExecutionGroupName.Contains(executionGroupName));
        }

        if (versionNumber != null)
        {
            executionGroupQuery = executionGroupQuery.Where(x => x.SoftwareVersionNumber == versionNumber);
        }

        executionGroupQuery = executionGroupQuery.OrderByDescending(x => x.SoftwareVersionNumber);
        executionGroupQuery = executionGroupQuery.Skip((pageNumber - 1) * pageSize).Take(pageSize);

        return await executionGroupQuery.ToListAsync();
    }

    public async Task<TeczterValidationResult<ExecutionGroupEntity>> ValidateExecutionGroup(ExecutionGroupEntity executionGroup)
    {
        var result = await _validator.ValidateAsync(executionGroup);

        if (!result.IsValid)
        {
            _uow.Rollback();
            return TeczterValidationResult<ExecutionGroupEntity>.Fail(result.Errors.Select(x => x.ErrorMessage).ToArray());
        }

        return TeczterValidationResult<ExecutionGroupEntity>.Succeed(executionGroup);
    }

    public async Task<TeczterValidationResult<ExecutionEntity>> DeleteExecution(ExecutionGroupEntity executionGroup, int executionId)
    {
        var execution = executionGroup.Executions.Single(x => x.Id == executionId && !x.IsDeleted);

        var result = executionGroup.DeleteExecution(execution);

        if (result.IsValid)
        {
            await _uow.SaveChanges(); 
        }

        return result;
    }
}
