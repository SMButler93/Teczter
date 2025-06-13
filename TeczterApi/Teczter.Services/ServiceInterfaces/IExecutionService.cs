using Teczter.Domain;
using Teczter.Domain.Entities;
using Teczter.Services.RequestDtos.Executions;

namespace Teczter.Services.ServiceInterfaces;

public interface IExecutionService
{
    Task<ExecutionEntity?> GetExecutionById(int id);
    Task<TeczterValidationResult<ExecutionEntity>> CompleteExecution(ExecutionEntity execution, CompleteExecutionRequestDto request);
    Task<TeczterValidationResult<ExecutionEntity>> ValidateExecutionState(ExecutionEntity execution);
}
