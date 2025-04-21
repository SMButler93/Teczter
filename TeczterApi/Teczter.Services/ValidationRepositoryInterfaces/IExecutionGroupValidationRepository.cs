using Teczter.Domain.Entities;

namespace Teczter.Services.ValidationRepositoryInterfaces;

public interface IExecutionGroupValidationRepository
{
    List<ExecutionGroupEntity> GetExecutionGroupsWithName(string name);
    List<ExecutionGroupEntity> GetExecutionGroupsWithSoftwareVersionNumber(string versionNumber);
}
