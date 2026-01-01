using Teczter.Domain;
using Teczter.Domain.Entities;
using Teczter.Services.RequestDtos;
using Teczter.Services.RequestDtos.ExecutionGroups;
using Teczter.Services.RequestDtos.Executions;

namespace Teczter.Services.ServiceInterfaces;

public interface IExecutionGroupService
{
    Task<List<ExecutionGroupEntity>> GetExecutionGroupSearchResults(int pageNumber, string? executionGroupName, string? versionNumber, CancellationToken ct);
    Task<ExecutionGroupEntity?> GetExecutionGroupById(int id, CancellationToken ct);
    Task<TeczterValidationResult<ExecutionGroupEntity>> DeleteExecutionGroup(ExecutionGroupEntity executionGroup, CancellationToken ct);
    Task<TeczterValidationResult<ExecutionEntity>> DeleteExecution(ExecutionGroupEntity executionGroup, int executionId, CancellationToken ct);
    Task<TeczterValidationResult<ExecutionGroupEntity>> CloneExecutionGroup(ExecutionGroupEntity executionGroupToClone, string newExecutionGroupName, string? softwareVersionNumber, CancellationToken ct);
    Task<TeczterValidationResult<ExecutionGroupEntity>> CreateExecution(ExecutionGroupEntity executionGroup, CreateExecutionRequestDto request, CancellationToken ct);
    Task<TeczterValidationResult<ExecutionGroupEntity>> CreateNewExecutionGroup(CreateExecutionGroupRequestDto request, CancellationToken ct);
    Task<TeczterValidationResult<ExecutionGroupEntity>> ValidateExecutionGroup(ExecutionGroupEntity executionGroup, CancellationToken ct);
}