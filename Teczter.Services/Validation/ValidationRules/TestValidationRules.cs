using Teczter.Domain;
using Teczter.Domain.Entities;

namespace Teczter.Services.Validation.ValidationRules;

public static class TestValidationRules
{
    public static CzValidationResult ValidateTitleIsNotEmpty(TestEntity test)
    {
        if (String.IsNullOrWhiteSpace(test.Title))
        {
            return CzValidationResult.Fail("A test must have a title.");
        }

        return CzValidationResult.Succeed();
    }

    public static CzValidationResult ValidateDescriptionIsNotEmpty(TestEntity test)
    {
        if (String.IsNullOrWhiteSpace(test.Description))
        {
            return CzValidationResult.Fail("A test must have a description.");
        }

        return CzValidationResult.Succeed();
    }
}
