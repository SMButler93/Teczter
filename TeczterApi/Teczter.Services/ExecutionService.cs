using FluentValidation;
using Teczter.Domain;
using Teczter.Domain.Entities;
using Teczter.Persistence;
using Teczter.Services.AdapterInterfaces;
using Teczter.Services.RequestDtos.Executions;
using Teczter.Services.ServiceInterfaces;

namespace Teczter.Services;

public class ExecutionService : IExecutionService
{
    private readonly IExecutionAdapter _executionAdapter;
    private readonly IUnitOfWork _uow;
    private readonly IValidator<ExecutionEntity> _validator;

    public ExecutionService(IExecutionAdapter executionAdapter, IUnitOfWork uow, IValidator<ExecutionEntity> validator)
    {
        _executionAdapter = executionAdapter;
        _uow = uow;
        _validator = validator;
    }

    public async Task<TeczterValidationResult<ExecutionEntity>> CompleteExecution(ExecutionEntity execution, CompleteExecutionRequestDto request)
    {
        if (request.HasPassed)
        {
            //Must pass in userId when setup.
            execution.Pass(default);
        } else
        {
            execution.Fail(default, (int)request.FailedStepId!, request.FailureReason!);
        }

        var result = await ValidateExecutionState(execution);

        await EvaluateValidationResultAndPersist(result);

        return result;
    }

    public async Task<TeczterValidationResult<ExecutionEntity>> ValidateExecutionState(ExecutionEntity execution)
    {
        var result = await _validator.ValidateAsync(execution);

        if (!result.IsValid)
        {
            return TeczterValidationResult<ExecutionEntity>.Fail(result.Errors.Select(x => x.ErrorMessage).ToArray());
        }

        return TeczterValidationResult<ExecutionEntity>.Succeed(execution);
    }

    private async Task EvaluateValidationResultAndPersist(TeczterValidationResult<ExecutionEntity> result)
    {
        if (!result.IsValid)
        {
            _uow.Rollback();
            return;
        }

        await _uow.CommitChanges();
    }
}
