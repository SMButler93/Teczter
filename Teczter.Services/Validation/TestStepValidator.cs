using FluentValidation;
using Teczter.Domain.Entities;

namespace Teczter.Services.Validation;

public class TestStepValidator : AbstractValidator<TestStepEntity>
{
    public TestStepValidator()
    {
        RuleFor(x => x.StepPlacement)
            .NotEmpty().WithMessage("A test step placement value must be provided for a test step.")
            .Must(x => x >= 1).WithMessage("A test step placement can not have a value less than 1.");

        RuleFor(x => x.Instructions)
            .NotEmpty().WithMessage("A test step must have instructions.");

        RuleFor(x => x.CreatedOn)
            .Must(x => x > DateTime.MinValue && x < DateTime.MaxValue)
            .WithMessage("Invalid Date. please provide a valid date.");

        RuleFor(x => x.RevisedOn)
            .Must(x => x > DateTime.MinValue && x < DateTime.MaxValue)
            .WithMessage("Invalid Date. please provide a valid date.");
    }
}
