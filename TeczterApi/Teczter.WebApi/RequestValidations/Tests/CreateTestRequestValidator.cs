using FluentValidation;
using Teczter.Services.RequestDtos.Tests;
using Teczter.Services.RequestDtos.TestSteps;
using Teczter.Services.Validation.ValidationRules;

namespace Teczter.WebApi.RequestValidations.Tests;

public class CreateTestRequestValidator : AbstractValidator<CreateTestRequestDto>
{
    public CreateTestRequestValidator(IValidator<CreateTestStepRequestDto> testStepValidator)
    {
        RuleFor(x => x.Title)
            .NotEmpty().WithMessage("A test must have a title.");

        RuleFor(x => x.Description)
            .NotEmpty().WithMessage("A test must have a description");

        RuleFor(x => x.OwningDepartment)
            .NotEmpty().WithMessage("A test must have an owning department")
            .Must(TestValidationRules.BeAValidDepartment).WithMessage("Invalid department. Please provide a valid department.");
        
        RuleFor(x => x.LinkUrls)
            .Must(TestValidationRules.HaveValidUrls).WithMessage("Invalid urls. Please ensure all urls are valid.");

        RuleForEach(x => x.TestSteps)
            .SetValidator(testStepValidator);
    }
}