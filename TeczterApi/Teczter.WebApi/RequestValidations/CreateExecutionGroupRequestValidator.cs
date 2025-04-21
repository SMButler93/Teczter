using FluentValidation;
using Teczter.Services.RequestDtos;
using Teczter.Services.Validation.ValidationRules;
using Teczter.Services.ValidationRepositoryInterfaces;

namespace Teczter.WebApi.RequestValidations;

public class CreateExecutionGroupRequestValidator : AbstractValidator<CreateExecutionGroupRequestDto>
{
    private readonly IExecutionGroupValidationRepository _validationRepository;
    private readonly IValidator<CreateExecutionRequestDto> _executionValidator;

    public CreateExecutionGroupRequestValidator(IExecutionGroupValidationRepository validationRepository, IValidator<CreateExecutionRequestDto> executionValidator)
    {
        _validationRepository = validationRepository;
        _executionValidator = executionValidator;

        RuleFor(x => x.ExecutionGroupName)
            .NotEmpty().WithMessage("An execution group must have a name.")
            .Must(x => ExecutionGroupValidationRules.BeUniqueExecutionGroupName(x, _validationRepository))
            .WithMessage("An execution group must have a unique name.");

        RuleFor(x => x.SoftwareVersionNumber)
            .Must(x => ExecutionGroupValidationRules.BeUniqueSoftwareVersionNumberOrNull(x, _validationRepository))
            .WithMessage("An execution group must have a unique software version number");

        RuleForEach(x => x.Executions)
            .SetValidator(_executionValidator);
    }
}
