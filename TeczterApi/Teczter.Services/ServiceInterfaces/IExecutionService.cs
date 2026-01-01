using Teczter.Domain;
using Teczter.Domain.Entities;
using Teczter.Services.RequestDtos.Executions;

namespace Teczter.Services.ServiceInterfaces;

public interface IExecutionService
{
    Task<TeczterValidationResult<ExecutionEntity>> CompleteExecution(ExecutionEntity execution, CompleteExecutionRequestDto request, CancellationToken ct);
    Task<TeczterValidationResult<ExecutionEntity>> ValidateExecutionState(ExecutionEntity execution, CancellationToken ct);
}
