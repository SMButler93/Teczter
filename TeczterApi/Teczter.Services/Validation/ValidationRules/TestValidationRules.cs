using Teczter.Domain.Entities;
using Teczter.Domain.Enums;
using Teczter.Services.ValidationRepositoryInterfaces;

namespace Teczter.Services.Validation.ValidationRules;

public static class TestValidationRules
{
    public static async Task<bool> BeUniqueTitle(string title, ITestValidationRepository repo)
    {
        var existingTests = await repo.GetTestEntitiesWithTitle(title);
        return existingTests.Count == 0;
    }

    public static async Task<bool> BeUniqueTitle(TestEntity test, ITestValidationRepository repo)
    {
        var existingTests = await repo.GetTestEntitiesWithTitle(test.Title);
        return existingTests.Count == 0 || (existingTests.Count == 1 && existingTests.First().Id == test.Id);
    }

    public static bool BeAValidDepartment(string department)
    {
        return Enum.TryParse<Department>(department, true, out var _);
    }

    public static bool HaveNoDuplicateStepPlacements(List<TestStepEntity> testSteps)
    {
        var distinctStepPlacements = testSteps.Select(x => x.StepPlacement).Distinct().ToList();
        return distinctStepPlacements.Count == testSteps.Count;
    }

    public static bool HaveNoMissingStepPlacements(List<TestStepEntity> testSteps)
    {
        var stepPlacements = testSteps.Select(x => x.StepPlacement).ToArray();

        for (var i = 1; i <= stepPlacements.Length; i++)
        {
            if (!stepPlacements.Contains(i))
            {
                return false;
            }
        }

        return true;
    }
}
