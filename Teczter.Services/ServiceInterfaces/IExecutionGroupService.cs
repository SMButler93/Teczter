using Teczter.Domain;
using Teczter.Domain.Entities;
using Teczter.Services.RequestDtos.Request;

namespace Teczter.Services.ServiceInterfaces;

public interface IExecutionGroupService
{
    Task<List<ExecutionGroupEntity>> GetExecutionGroupSearchResults(string? executionGroupName, string? versionNumber);
    Task<ExecutionGroupEntity?> GetExecutionGroupById(int id);
    Task DeleteExecutionGroup(ExecutionGroupEntity executionGroup);
    Task<TeczterValidationResult<ExecutionGroupEntity>> CloneExecutionGroup(ExecutionGroupEntity executiongroupToClone, string NewExecutionGroupName, string? softwareVersionNumber);
    Task<TeczterValidationResult<ExecutionGroupEntity>> CreateExecution(ExecutionGroupEntity executionGroup, CreateExecutionRequestDto request);
    Task<TeczterValidationResult<ExecutionGroupEntity>> CreateNewExecutionGroup(CreateExecutionGroupRequestDto request);
    Task<TeczterValidationResult<ExecutionGroupEntity>> ValidateExecutionGroup(ExecutionGroupEntity executiongroup);
}
