using System.Linq;
using Teczter.Adapters.ValidationRepositories.TestValidationRespositories;
using Teczter.Domain.Entities;
using Teczter.Domain.Enums;

namespace Teczter.Services.Validation.ValidationRules;

public static class TestValidationRules
{
    public static async Task<bool> BeUniqueTitle(string title, ITestValidationRepository repo, CancellationToken token)
    {
        var existingTests = await repo.GetTestEntitiesWithTitle(title);
        return existingTests.Count == 0;
    }

    public static bool BeAValidPillar(string pillar)
    {
        var validPillars = Enum.GetNames(typeof(Pillar)).Select(x => x.ToLower());
        return validPillars.Contains(pillar.ToLower());
    }

    public static bool HaveNoDuplicateStepPlacements(List<TestStepEntity> testSteps)
    {
        var distinctStepPlacements = testSteps.Select(x => x.StepPlacement).ToHashSet();
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
