using FluentValidation;
using Teczter.Domain.Entities;

namespace Teczter.Services.Validation;

public class ExecutionValidator : AbstractValidator<ExecutionEntity>
{
    public ExecutionValidator()
    {
        RuleFor(x => x.ExecutionGroupId)
            .NotEmpty().WithMessage("An execution must belong to a group.");

        RuleFor(x => x.TestId)
            .NotEmpty().WithMessage("An execution must have an associated test.");
    }
}
