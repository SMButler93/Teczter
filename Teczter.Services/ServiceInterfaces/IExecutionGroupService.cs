using Teczter.Domain.Entities;
using Teczter.Services.RequestDtos.Request;

namespace Teczter.Services.ServiceInterfaces;

public interface IExecutionGroupService
{
    Task<List<ExecutionGroupEntity>> GetExecutionGroupSearchResults(string? executionGroupName, string? versionNumber);
    Task<ExecutionGroupEntity?> GetExecutionGroupById(int id);
    Task DeleteExecutionGroup(ExecutionGroupEntity executionGroup);
    Task<ExecutionGroupEntity> CloneExecutionGroup(ExecutionGroupEntity executiongroupToClone, string NewExecutionGroupName, string? softwareVersionNumber);
    Task<ExecutionGroupEntity> CreateNewExecutionGroup(CreateExecutionGroupRequestDto executiongroup);
}
