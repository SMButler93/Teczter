﻿using FluentValidation;
using Teczter.Adapters.ValidationRepositories.ExecutionGroupValidationRepositories;
using Teczter.Domain.Entities;
using Teczter.Services.Validation.ValidationRules;

namespace Teczter.Services.Validation;

public class ExecutionGroupValidator : AbstractValidator<ExecutionGroupEntity>
{
    private readonly IExecutionGroupValidationRepository _validationRepository;

    public ExecutionGroupValidator(IExecutionGroupValidationRepository validationRepository)
    {
        _validationRepository = validationRepository;

        RuleFor(x => x.ExecutionGroupName)
            .Must(x => ExecutionGroupValidationRules.BeUniqueExecutionGroupName(x, _validationRepository))
            .WithMessage("An execution group must have a unique name.");

        RuleFor(x => x.SoftwareVersionNumber)
            .Must(x => ExecutionGroupValidationRules.BeUniqueSoftwareVersionNumberOrNull(x, _validationRepository))
            .WithMessage("An execution group must have a unique software version number.");

        RuleFor(x => x.CreatedOn)
            .Must(y => y > DateTime.MinValue).WithMessage("Invalid Date. please provide a valid date.")
            .Must(y => y < DateTime.MaxValue).WithMessage("Invalid Date. please provide a valid date.");

        RuleFor(x => x.RevisedOn)
            .Must(y => y > DateTime.MinValue).WithMessage("Invalid Date. please provide a valid date.")
            .Must(y => y < DateTime.MaxValue).WithMessage("Invalid Date. please provide a valid date.");
    }
}