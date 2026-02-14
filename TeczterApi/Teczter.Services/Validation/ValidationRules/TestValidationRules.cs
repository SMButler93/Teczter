using Teczter.Domain.Entities;
using Teczter.Domain.Enums;

namespace Teczter.Services.Validation.ValidationRules;

public static class TestValidationRules
{
    public static bool BeAValidDepartment(string department)
    {
        return Enum.TryParse<Department>(department, true, out var _);
    }

    public static bool HaveNoDuplicateStepPlacements(List<TestStepEntity> testSteps)
    {
        var distinctStepPlacements = testSteps.Select(x => x.StepPlacement).Distinct().ToList();
        return distinctStepPlacements.Count == testSteps.Count;
    }

    public static bool HaveValidUrls(ICollection<string> urls)
    {
        Predicate<string> isValid = url => Uri.TryCreate(url, UriKind.Absolute, out var uriResult) 
                                           && (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps);

        return urls.All(url => isValid(url));
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
