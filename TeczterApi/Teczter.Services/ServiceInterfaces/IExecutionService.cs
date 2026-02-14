using Teczter.Domain;
using Teczter.Domain.Entities;
using Teczter.Services.RequestDtos.Executions;

namespace Teczter.Services.ServiceInterfaces;

public interface IExecutionService
{
    Task<ExecutionEntity?> GetExecutionById(int executionId, CancellationToken ct);
    Task<TeczterValidationResult<ExecutionEntity>> CompleteExecution(ExecutionEntity execution, CompleteExecutionRequestDto request, CancellationToken ct);
    Task<TeczterValidationResult<ExecutionEntity>> DeleteExecution(ExecutionEntity execution, CancellationToken ct);
}
