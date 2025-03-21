using Teczter.Domain.Entities;

namespace Teczter.Adapters.ValidationRepositories.ExecutionGroupValidationRepositories;

public interface IExecutionGroupValidationRepository
{
    List<ExecutionGroupEntity> GetExecutionGroupsWithName(string name);
    List<ExecutionGroupEntity> GetExecutionGroupsWithSoftwareVersionNumber(string versionNumber);
}
