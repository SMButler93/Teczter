using FluentValidation;
using Teczter.Adapters.ValidationRepositories.TestValidationRespositories;
using Teczter.Services.DTOs.Request;
using Teczter.Services.Validation.ValidationRules;

namespace Teczter.WebApi.RequestValidations;

public class CreateTestRequestValidator : AbstractValidator<CreateTestRequestDto>
{
    private readonly ITestValidationRepository _testValidationRepository;

    public CreateTestRequestValidator(ITestValidationRepository testValidationrespository)
    {
        _testValidationRepository = testValidationrespository;

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
            .SetValidator(new TestStepCommandRequestValidator());
    }
}