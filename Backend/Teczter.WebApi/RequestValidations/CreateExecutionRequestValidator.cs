﻿using FluentValidation;
using Teczter.Services.RequestDtos;

namespace Teczter.WebApi.RequestValidations;

public class CreateExecutionRequestValidator : AbstractValidator<CreateExecutionRequestDto>
{
    public CreateExecutionRequestValidator()
    {
        RuleFor(x => x.TestId)
            .NotEmpty().WithMessage("An execution must have a valid Test ID.");
    }
}
