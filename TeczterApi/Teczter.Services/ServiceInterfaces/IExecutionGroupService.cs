using Teczter.Domain;
using Teczter.Domain.Entities;
using Teczter.Services.RequestDtos;
using Teczter.Services.RequestDtos.ExecutionGroups;
using Teczter.Services.RequestDtos.Executions;

namespace Teczter.Services.ServiceInterfaces;

public interface IExecutionGroupService
{
    Task<List<ExecutionGroupEntity>> GetExecutionGroupSearchResults(int pageNumber, string? executionGroupName, string? versionNumber);
    Task<ExecutionGroupEntity?> GetExecutionGroupById(int id);
    Task<TeczterValidationResult<ExecutionGroupEntity>> DeleteExecutionGroup(ExecutionGroupEntity executionGroup);
    Task<TeczterValidationResult<ExecutionEntity>> DeleteExecution(ExecutionGroupEntity executionGroup, int executionId);
    Task<TeczterValidationResult<ExecutionGroupEntity>> CloneExecutionGroup(ExecutionGroupEntity executiongroupToClone, string NewExecutionGroupName, string? softwareVersionNumber);
    Task<TeczterValidationResult<ExecutionGroupEntity>> CreateExecution(ExecutionGroupEntity executionGroup, CreateExecutionRequestDto request);
    Task<TeczterValidationResult<ExecutionGroupEntity>> CreateNewExecutionGroup(CreateExecutionGroupRequestDto request);
    Task<TeczterValidationResult<ExecutionGroupEntity>> ValidateExecutionGroup(ExecutionGroupEntity executiongroup);
}