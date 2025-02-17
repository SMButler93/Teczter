using Teczter.Domain;
using Teczter.Domain.Entities;

namespace Teczter.Services.Validation.ValidationRules;

public static class TestValidationRules
{
    public static TeczterValidationResult ValidateTitleIsNotEmpty(TestEntity test)
    {
        if (String.IsNullOrWhiteSpace(test.Title))
        {
            return TeczterValidationResult.Fail("A test must have a title.");
        }

        return TeczterValidationResult.Succeed();
    }

    public static TeczterValidationResult ValidateDescriptionIsNotEmpty(TestEntity test)
    {
        if (String.IsNullOrWhiteSpace(test.Description))
        {
            return TeczterValidationResult.Fail("A test must have a description.");
        }

        return TeczterValidationResult.Succeed();
    }
}
