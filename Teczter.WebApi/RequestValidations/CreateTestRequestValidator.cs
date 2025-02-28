using FluentValidation;
using Teczter.Adapters.ValidationRepositories.TestValidationRespositories;
using Teczter.Domain.Enums;
using Teczter.Services.DTOs.Request;
using Teczter.Services.ServiceInterfaces;

namespace Teczter.WebApi.RequestValidations;

public class CreateTestRequestValidator : AbstractValidator<CreateTestRequestDto>
{
    private readonly ITestValidationRepository _testValidationRepository;

    public CreateTestRequestValidator(ITestValidationRepository testValidationrespository)
    {
        _testValidationRepository = testValidationrespository;

        RuleFor(x => x.Title)
            .NotEmpty().WithMessage("A test must have a title.")
            .Must(BeUniqueTitle).WithMessage("A test must have a unique title.");

        RuleFor(x => x.Description)
            .NotEmpty().WithMessage("A test must have a description");

        RuleFor(x => x.OwningDepartment)
            .NotEmpty().WithMessage("A test must have an owning department")
            .Must(BeAValidDepartment).WithMessage("Invalid department. Please provide a valid department.");

        RuleForEach(x => x.TestSteps)
            .SetValidator(new TestStepCommandRequestValidator());
    }

    private bool BeUniqueTitle(string title)
    {
        var existingTests = _testValidationRepository.GetTestEntitiesWithTitle(title).Result;
        return existingTests.Count == 0;
    }

    private bool BeAValidDepartment(string department)
    {
        var validDepartments = Enum.GetNames(typeof(Department)).Select(x => x.ToLower());
        return validDepartments.Contains(department.ToLower());
    }
}