using FluentValidation;
using Teczter.Services.RequestDtos.ExecutionGroups;
using Teczter.Services.RequestDtos.Executions;
using Teczter.Services.ValidationRepositoryInterfaces;

namespace Teczter.WebApi.RequestValidations.ExecutionGroups;

public class CreateExecutionGroupRequestValidator : AbstractValidator<CreateExecutionGroupRequestDto>
{
    private readonly IExecutionGroupValidationRepository _validationRepository;
    private readonly IValidator<CreateExecutionRequestDto> _executionValidator;

    public CreateExecutionGroupRequestValidator(IExecutionGroupValidationRepository validationRepository, IValidator<CreateExecutionRequestDto> executionValidator)
    {
        _validationRepository = validationRepository;
        _executionValidator = executionValidator;

        RuleFor(x => x.ExecutionGroupName)
            .NotEmpty().WithMessage("An execution group must have a name.");

        RuleFor(x => x.SoftwareVersionNumber)
            .NotEmpty().WithMessage("An execution group must have a unique software version number");

        RuleForEach(x => x.Executions)
            .SetValidator(_executionValidator);
    }
}
