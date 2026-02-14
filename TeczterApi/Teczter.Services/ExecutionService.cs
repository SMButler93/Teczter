using FluentValidation;
using Teczter.Domain;
using Teczter.Domain.Entities;
using Teczter.Persistence;
using Teczter.Services.AdapterInterfaces;
using Teczter.Services.RequestDtos.Executions;
using Teczter.Services.ServiceInterfaces;

namespace Teczter.Services;

public class ExecutionService(
    IExecutionAdapter _executionAdapter,
    IUnitOfWork _uow,
    IValidator<ExecutionEntity> _validator)
    : IExecutionService
{
    public async Task<ExecutionEntity?> GetExecutionById(int executionId, CancellationToken ct)
    {
        return await _executionAdapter.GetExecutionById(executionId, ct);
    }
    
    public async Task<TeczterValidationResult<ExecutionEntity>> CompleteExecution(int id, CompleteExecutionRequestDto request, CancellationToken ct)
    {
        var execution = await _executionAdapter.GetExecutionById(id, ct);

        if (execution is null)
        {
            return TeczterValidationResult<ExecutionEntity>.Fail($"No active execution found with ID {id}.");
        }
        
        if (!execution.ExecutionGroup.CanModify)
        {
            return TeczterValidationResult<ExecutionEntity>.Fail("Cannot modify an execution that belongs to a closed or deleted execution group.");
        }

        if (request.HasPassed)
        {
            execution.Pass(default);
        }
        else
        {
            execution.Fail(default, (int)request.FailedStepId!, request.FailureReason!);
        }

        var result = await ValidateExecutionState(execution, ct);

        if (result.IsValid) await _uow.SaveChanges(ct);

        return result;
    }

    public async Task<TeczterValidationResult<ExecutionEntity>> DeleteExecution(int id, CancellationToken ct)
    {
        var execution = await _executionAdapter.GetExecutionById(id, ct);

        if (execution is null)
        {
            return TeczterValidationResult<ExecutionEntity>.Fail($"No active execution found with ID {id}");
        }
        
        if (!execution.ExecutionGroup.CanModify)
        {
            return TeczterValidationResult<ExecutionEntity>.Fail("Cannot modify an execution that belongs to a closed or deleted execution group.");
        }
        
        execution.Delete();
        
        var result = await ValidateExecutionState(execution, ct);
        
        if (result.IsValid) await _uow.SaveChanges(ct);
        return result;
    }

    private async Task<TeczterValidationResult<ExecutionEntity>> ValidateExecutionState(ExecutionEntity execution, CancellationToken ct)
    {
        var result = await _validator.ValidateAsync(execution, ct);

        return result.IsValid
            ? TeczterValidationResult<ExecutionEntity>.Succeed(execution)
            : TeczterValidationResult<ExecutionEntity>.Fail(result.Errors.Select(x => x.ErrorMessage).ToArray());
    }
}
