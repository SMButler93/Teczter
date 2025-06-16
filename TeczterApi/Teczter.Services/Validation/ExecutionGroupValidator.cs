using FluentValidation;
using Teczter.Domain.Entities;

namespace Teczter.Services.Validation;

public class ExecutionGroupValidator : AbstractValidator<ExecutionGroupEntity>
{
    private readonly IValidator<ExecutionEntity> _executionValidator;

    public ExecutionGroupValidator(IValidator<ExecutionEntity> executionValidator)
    {
        _executionValidator = executionValidator;

        RuleFor(x => x.CreatedOn)
            .Must(y => y > DateTime.MinValue).WithMessage("Invalid Date. please provide a valid date.")
            .Must(y => y < DateTime.MaxValue).WithMessage("Invalid Date. please provide a valid date.");

        RuleFor(x => x.RevisedOn)
            .Must(y => y > DateTime.MinValue).WithMessage("Invalid Date. please provide a valid date.")
            .Must(y => y < DateTime.MaxValue).WithMessage("Invalid Date. please provide a valid date.");

        RuleForEach(x => x.Executions)
            .SetValidator(_executionValidator);
    }
}