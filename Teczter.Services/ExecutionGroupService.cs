﻿using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Teczter.Adapters.AdapterInterfaces;
using Teczter.Domain;
using Teczter.Domain.Entities;
using Teczter.Domain.Exceptions;
using Teczter.Persistence;
using Teczter.Services.RequestDtos;
using Teczter.Services.ServiceInterfaces;

namespace Teczter.Services;

public class ExecutionGroupService : IExecutionGroupService
{
    private readonly IExecutionGroupAdapter _executionGroupAdapter;
    private readonly IExecutionGroupBuilder _builder;
    private readonly IValidator<ExecutionGroupEntity> _validator;
    private readonly IUnitOfWork _uow;

    private const int pageSize = 15;

    public ExecutionGroupService(
        IExecutionGroupAdapter executionGroupAdapter, 
        IExecutionGroupBuilder builder,
        IValidator<ExecutionGroupEntity> validator,
        IUnitOfWork uow)
    {
        _executionGroupAdapter = executionGroupAdapter;
        _builder = builder;
        _validator = validator;
        _uow = uow;
    }

    public async Task<TeczterValidationResult<ExecutionGroupEntity>> CloneExecutionGroup(
        ExecutionGroupEntity executiongroupToClone, 
        string newExecutionGroupName, string? 
        softwareVersionNumber)
    {
        var newExecutionGroup = executiongroupToClone.CloneExecutionGroup(newExecutionGroupName, softwareVersionNumber);

        await _executionGroupAdapter.CreateNewExecutionGroup(newExecutionGroup);

        var result = await ValidateExecutionGroup(newExecutionGroup);

        await EvaluateValidationResultAndPersist(result);
        return result;
    }

    public async Task<TeczterValidationResult<ExecutionGroupEntity>> CreateExecution(ExecutionGroupEntity executionGroup, CreateExecutionRequestDto request)
    {
        var group = _builder
            .UsingContext(executionGroup)
            .AddExecution(request)
            .Build();

        var result = await ValidateExecutionGroup(group);

        await EvaluateValidationResultAndPersist(result);
        return result;
    }

    public async Task<TeczterValidationResult<ExecutionGroupEntity>> CreateNewExecutionGroup(CreateExecutionGroupRequestDto request)
    {
        var group = _builder
            .NewInstance()
            .SetName(request.ExecutionGroupName)
            .SetSoftwareVersionNumber(request.SoftwareVersionNumber)
            .SetExecutionGroupNotes(request.ExecutionGroupNotes)
            .AddExecutions(request.Executions)
            .Build();

        await _executionGroupAdapter.CreateNewExecutionGroup(group);

        var result = await ValidateExecutionGroup(group);

        await EvaluateValidationResultAndPersist(result);
        return result;
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

    public async Task<TeczterValidationResult<ExecutionGroupEntity>> ValidateExecutionGroup(ExecutionGroupEntity executiongroup)
    {
        var result = await _validator.ValidateAsync(executiongroup);

        if (!result.IsValid)
        {
            _uow.Rollback();
            return TeczterValidationResult<ExecutionGroupEntity>.Fail(result.Errors.Select(x => x.ErrorMessage).ToArray());
        }

        return TeczterValidationResult<ExecutionGroupEntity>.Succeed(executiongroup);
    }

    private async Task EvaluateValidationResultAndPersist(TeczterValidationResult<ExecutionGroupEntity> result)
    {
        if (!result.IsValid)
        {
            _uow.Rollback();
            return;
        }

        await _uow.CommitChanges();
    }

    public async Task RemoveExecution(ExecutionGroupEntity executionGroup, int executionId)
    {
        var execution = executionGroup.Executions.SingleOrDefault(x => x.Id == executionId) ??
            throw new TeczterValidationException("Cannot remove an execution that does not exist, has already been deleted, " +
            "or does not belong to this execution group.");

        executionGroup.RemoveExecution(execution);

        await _uow.CommitChanges();
    }

    public async Task<ExecutionEntity?> GetExecutionByIdAndGroupId(int executionGroupId, int executionId)
    {
        var executionGroup = await _executionGroupAdapter.GetExecutionGroupById(executionGroupId);

        return executionGroup?.Executions.SingleOrDefault(x => x.Id == executionId);
    }
}
