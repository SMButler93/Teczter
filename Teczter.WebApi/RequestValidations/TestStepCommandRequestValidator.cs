using FluentValidation;
using Teczter.Services.DTOs.Request;

namespace Teczter.WebApi.RequestValidations;

public class TestStepCommandRequestValidator : AbstractValidator<TestStepCommandRequestDto>
{
    public TestStepCommandRequestValidator()
    {
        RuleFor(x => x.StepPlacement)
        .NotEmpty().WithMessage("A test step placement value must be provided for a test step.")
        .Must(x => x >= 1).WithMessage("A test step placement can not have a value less than 1.");

        RuleFor(x => x.Instructions)
            .NotEmpty().WithMessage("A test step must have instructions.");
    }
}
