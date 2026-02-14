using FluentValidation;
using Teczter.Services.RequestDtos.ExecutionGroups;
using Teczter.Services.RequestDtos.Executions;

namespace Teczter.WebApi.RequestValidations.ExecutionGroups;

public class CreateExecutionGroupRequestValidator : AbstractValidator<CreateExecutionGroupRequestDto>
{
    public CreateExecutionGroupRequestValidator(IValidator<CreateExecutionRequestDto> executionValidator)
    {
        RuleFor(x => x.ExecutionGroupName)
            .NotEmpty().WithMessage("An execution group must have a name.");

        RuleFor(x => x.SoftwareVersionNumber)
            .NotEmpty().WithMessage("An execution group must have a unique software version number");

        RuleForEach(x => x.Executions)
            .SetValidator(executionValidator);
    }
}
