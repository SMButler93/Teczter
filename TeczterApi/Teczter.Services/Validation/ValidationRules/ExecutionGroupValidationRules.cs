using Teczter.Domain.Entities;
using Teczter.Services.ValidationRepositoryInterfaces;

namespace Teczter.Services.Validation.ValidationRules;

public static class ExecutionGroupValidationRules
{
    public static bool BeUniqueExecutionGroupName(string executionGroupName, IExecutionGroupValidationRepository repo)
    {
        var existingExecutionGroups = repo.GetExecutionGroupsWithName(executionGroupName);
        return existingExecutionGroups.Count == 0;
    }

    public static bool BeUniqueExecutionGroupName(ExecutionGroupEntity executionGroup, IExecutionGroupValidationRepository repo)
    {
        var existingExecutionGroups = repo.GetExecutionGroupsWithName(executionGroup.ExecutionGroupName);
        return existingExecutionGroups.Count == 0 || (existingExecutionGroups.Count == 1 && existingExecutionGroups.First().Id == executionGroup.Id);
    }

    public static bool BeUniqueSoftwareVersionNumberOrNull(string? versionNumber, IExecutionGroupValidationRepository repo)
    {
        if (versionNumber is null)
        {
            return true;
        }

        var existingExecutionGroups = repo.GetExecutionGroupsWithSoftwareVersionNumber(versionNumber);
        return existingExecutionGroups.Count == 0;
    }

    public static bool BeUniqueSoftwareVersionNumberOrNull(ExecutionGroupEntity executionGroup, IExecutionGroupValidationRepository repo)
    {
        if (executionGroup.SoftwareVersionNumber is null)
        {
            return true;
        }

        var existingExecutionGroups = repo.GetExecutionGroupsWithSoftwareVersionNumber(executionGroup.SoftwareVersionNumber);
        return existingExecutionGroups.Count == 0 || (existingExecutionGroups.Count == 1 && existingExecutionGroups.First().Id == executionGroup.Id);
    }
}