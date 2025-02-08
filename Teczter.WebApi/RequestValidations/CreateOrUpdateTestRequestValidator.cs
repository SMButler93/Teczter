using FluentValidation;
using Teczter.Domain.Enums;
using Teczter.Services.DTOs.Request;

namespace Teczter.WebApi.RequestValidations;

public class CreateOrUpdateTestRequestValidator : AbstractValidator<TestCommandRequestDto>
{
    private static readonly string[] Pillars = Enum.GetNames(typeof(Pillar));

    public CreateOrUpdateTestRequestValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty()
            .WithMessage("Test titles cannot be empty.");

        RuleFor(x => x.Description)
            .NotEmpty()
            .WithMessage("Test description cannot be empty");

        RuleFor(x => x.Pillar)
            .NotEmpty()
            .Must(OwningPillarIsValidValue)
            .WithMessage("A test must have a valid pillar assigned.");
    }

    private bool OwningPillarIsValidValue(string pillar)
    {
        return Pillars.Contains(pillar.ToUpper());
    }
}