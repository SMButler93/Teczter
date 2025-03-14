using Teczter.Domain.Entities;

namespace Teczter.Services.ServiceInterfaces;

public interface IExecutionGroupService
{
    Task<List<ExecutionGroupEntity>> GetExecutionGroupSearchResults(string? executionGroupName, string? versionNumber);
    Task<ExecutionGroupEntity?> GetExecutionGroupById(int id);
    Task DeleteExecutionGroup(ExecutionGroupEntity executionGroup);
}
