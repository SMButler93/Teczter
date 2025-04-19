using FluentValidation;
using Teczter.Services.RequestDtos;
using Teczter.Services.Validation.ValidationRules;
using Teczter.Services.ValidationRepositoryInterfaces;

namespace Teczter.WebApi.RequestValidations;

public class UpdateTestRequestValidator : AbstractValidator<UpdateTestRequestDto>
{
    private readonly ITestValidationRepository _testValidationRepository;
    public UpdateTestRequestValidator(ITestValidationRepository testValidationRepository)
    {
        _testValidationRepository = testValidationRepository;

        RuleFor(x => x.Title)
            .Must(x => x == null || TestValidationRules.BeUniqueTitle(x, _testValidationRepository))
            .WithMessage("This title is already being used. A test must have a unique title.");

        RuleFor(x => x.OwningDepartment)
            .Must(x => x == null || TestValidationRules.BeAValidDepartment(x))
            .WithMessage("Invalid department. Please provide a valid department.");
    }
}