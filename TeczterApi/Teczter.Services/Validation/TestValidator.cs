using FluentValidation;
using Teczter.Domain.Entities;
using Teczter.Services.Validation.ValidationRules;

namespace Teczter.Services.Validation;

public class TestValidator : AbstractValidator<TestEntity>
{
    public TestValidator(IValidator<TestStepEntity> _testStepValidator)
    {
        RuleFor(x => x.Title)
            .NotEmpty().WithMessage("A test must have a title.");

        RuleFor(x => x.Description)
            .NotEmpty().WithMessage("A test must have a description");

        RuleFor(x => x.OwningDepartment)
            .NotEmpty().WithMessage("A test must have an owning department.");
        
        RuleFor(x => x.Urls)
            .Must(TestValidationRules.HaveValidUrls).WithMessage("Invalid urls. Please ensure all urls are valid.");

        RuleFor(x => x.CreatedOn)
            .Must(y => y > DateTime.MinValue).WithMessage("Invalid Date. please provide a valid date.")
            .Must(y => y < DateTime.MaxValue).WithMessage("Invalid Date. please provide a valid date.");

        RuleFor(x => x.RevisedOn)
            .Must(y => y > DateTime.MinValue).WithMessage("Invalid Date. please provide a valid date.")
            .Must(y => y < DateTime.MaxValue).WithMessage("Invalid Date. please provide a valid date.");

        RuleFor(x => x.TestSteps)
            .Must(TestValidationRules.HaveNoDuplicateStepPlacements).WithMessage("An error has occured with the test step placements being duplicated.")
            .Must(TestValidationRules.HaveNoMissingStepPlacements).WithMessage("An error has occured due to test step placements not being sequential.");

        RuleForEach(x => x.TestSteps)
            .SetValidator(_testStepValidator);
    }
}
