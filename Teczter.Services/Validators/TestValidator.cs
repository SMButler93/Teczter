using Teczter.Domain;
using Teczter.Domain.Entities;
using Teczter.Services.Validators.ValidatorAbstractions;

namespace Teczter.Services.ValidationServices;

public class TestValidator : CzAbstractValidator<TestEntity>
{
    public TestValidator()
    {
        _validationRules = 
            [
                ValidateTestTitleIsValid,
                ValidateTestDescriptionIsValid
            ];
    }

    private CzValidationResult ValidateTestTitleIsValid(TestEntity test)
    {
        if (String.IsNullOrEmpty(test.Title) || String.IsNullOrWhiteSpace(test.Title))
        {
            return CzValidationResult.Fail("The test title can not be left empty or just white space.");
        }

        return CzValidationResult.Succeed();
    }

    private CzValidationResult ValidateTestDescriptionIsValid(TestEntity test)
    {
        if (String.IsNullOrEmpty(test.Description) || String.IsNullOrWhiteSpace(test.Description))
        {
            return CzValidationResult.Fail("The test description can not be left empty or just white space.");
        }

        return CzValidationResult.Succeed();
    }
}
