using FluentValidation;
using Teczter.Services.RequestDtos.Tests;
using Teczter.Services.RequestDtos.TestSteps;
using Teczter.Services.Validation.ValidationRules;
using Teczter.Services.ValidationRepositoryInterfaces;

namespace Teczter.WebApi.RequestValidations.Tests;

public class CreateTestRequestValidator : AbstractValidator<CreateTestRequestDto>
{
    private readonly ITestValidationRepository _testValidationRepository;
    private readonly IValidator<CreateTestStepRequestDto> _testStepValidator;

    public CreateTestRequestValidator(ITestValidationRepository testValidationRepository, IValidator<CreateTestStepRequestDto> testStepValidator)
    {
        _testValidationRepository = testValidationRepository;
        _testStepValidator = testStepValidator;

        RuleFor(x => x.Title)
            .NotEmpty().WithMessage("A test must have a title.");

        RuleFor(x => x.Description)
            .NotEmpty().WithMessage("A test must have a description");

        RuleFor(x => x.OwningDepartment)
            .NotEmpty().WithMessage("A test must have an owning department")
            .Must(TestValidationRules.BeAValidDepartment).WithMessage("Invalid department. Please provide a valid department.");

        RuleForEach(x => x.TestSteps)
            .SetValidator(_testStepValidator);
    }
}