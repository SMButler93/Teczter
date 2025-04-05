using FluentValidation;
using Teczter.Adapters.ValidationRepositories.TestValidationRespositories;
using Teczter.Services.RequestDtos;
using Teczter.Services.Validation.ValidationRules;

namespace Teczter.WebApi.RequestValidations;

public class CreateTestRequestValidator : AbstractValidator<CreateTestRequestDto>
{
    private readonly ITestValidationRepository _testValidationRepository;
    private readonly IValidator<CreateTestStepRequestDto> _testStepValidator;

    public CreateTestRequestValidator(ITestValidationRepository testValidationRepository, IValidator<CreateTestStepRequestDto> testStepValidator)
    {
        _testValidationRepository = testValidationRepository;
        _testStepValidator = testStepValidator;

        RuleFor(x => x.Title)
            .NotEmpty().WithMessage("A test must have a title.")
            .Must(x => TestValidationRules.BeUniqueTitle(x, _testValidationRepository))
            .WithMessage("This title is already being used. A test must have a unique title.");

        RuleFor(x => x.Description)
            .NotEmpty().WithMessage("A test must have a description");

        RuleFor(x => x.OwningDepartment)
            .NotEmpty().WithMessage("A test must have an owning department")
            .Must(TestValidationRules.BeAValidDepartment).WithMessage("Invalid department. Please provide a valid department.");

        RuleForEach(x => x.TestSteps)
            .SetValidator(_testStepValidator);
    }
}