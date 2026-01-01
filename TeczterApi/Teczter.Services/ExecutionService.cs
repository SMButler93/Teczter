using FluentValidation;
using Teczter.Domain;
using Teczter.Domain.Entities;
using Teczter.Persistence;
using Teczter.Services.AdapterInterfaces;
using Teczter.Services.RequestDtos.Executions;
using Teczter.Services.ServiceInterfaces;

namespace Teczter.Services;

public class ExecutionService(
    IUnitOfWork uow,
    IValidator<ExecutionEntity> validator)
    : IExecutionService
{
    public async Task<TeczterValidationResult<ExecutionEntity>> CompleteExecution(ExecutionEntity execution, CompleteExecutionRequestDto request, CancellationToken ct)
    {
        if (request.HasPassed)
        {
            //Must provide a userId when setup.
            execution.Pass(default);
        } else
        {
            execution.Fail(default, (int)request.FailedStepId!, request.FailureReason!);
        }

        var result = await ValidateExecutionState(execution, ct);

        if (result.IsValid)
        {
            await uow.SaveChanges(ct);
        }

        return result;
    }

    public async Task<TeczterValidationResult<ExecutionEntity>> ValidateExecutionState(ExecutionEntity execution, CancellationToken ct)
    {
        var result = await validator.ValidateAsync(execution, ct);

        if (!result.IsValid)
        {
            return TeczterValidationResult<ExecutionEntity>.Fail(result.Errors.Select(x => x.ErrorMessage).ToArray());
        }

        return TeczterValidationResult<ExecutionEntity>.Succeed(execution);
    }
}
