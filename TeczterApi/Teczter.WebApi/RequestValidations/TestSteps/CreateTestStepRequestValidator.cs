using FluentValidation;
using Teczter.Services.RequestDtos.TestSteps;

namespace Teczter.WebApi.RequestValidations.TestSteps;

public class CreateTestStepRequestValidator : AbstractValidator<CreateTestStepRequestDto>
{
    public CreateTestStepRequestValidator()
    {
        RuleFor(x => x.StepPlacement)
        .NotEmpty().WithMessage("A test step placement value must be provided for a test step.")
        .Must(x => x >= 1).WithMessage("A test step placement can not have a value less than 1.");

        RuleFor(x => x.Instructions)
            .NotEmpty().WithMessage("A test step must have instructions.");
    }
}
