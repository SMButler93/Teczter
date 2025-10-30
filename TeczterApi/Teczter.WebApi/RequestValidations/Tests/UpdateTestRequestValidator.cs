using FluentValidation;
using Teczter.Services.RequestDtos.TestSteps;
using Teczter.Services.Validation.ValidationRules;

namespace Teczter.WebApi.RequestValidations.Tests;

public class UpdateTestRequestValidator : AbstractValidator<UpdateTestRequestDto>
{
    public UpdateTestRequestValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty()
            .WithMessage("A title must be provided.");

        RuleFor(x => x.Description)
            .NotEmpty()
            .WithMessage("A test must have a description");

        RuleFor(x => x.OwningDepartment)
            .NotEmpty().WithMessage("A test must have an owning department")
            .Must(TestValidationRules.BeAValidDepartment).WithMessage("Invalid department. Please provide a valid department.");
    }
}