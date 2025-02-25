using FluentValidation;
using Teczter.Adapters.ValidationRepositories.TestValidationRespositories;
using Teczter.Domain.Enums;
using Teczter.Services.DTOs.Request;
using Teczter.Services.ServiceInterfaces;

namespace Teczter.WebApi.RequestValidations;

public class TestCommandRequestValidator : AbstractValidator<TestCommandRequestDto>
{
    private readonly ITestValidationRepository _testValidationRepository;

    public TestCommandRequestValidator(ITestValidationRepository testValidationrespository)
    {
        _testValidationRepository = testValidationrespository;

        RuleFor(x => x.Title)
            .NotEmpty().WithMessage("A test must have a title.")
            .Must(BeUniqueTitle).WithMessage("A test must have a unique title.");

        RuleFor(x => x.Description)
            .NotEmpty().WithMessage("A test must have a description");

        RuleFor(x => x.OwningPillar)
            .NotEmpty().WithMessage("A test must have an owning pillar")
            .Must(BeAValidPillar).WithMessage("Invalid pillar. Please provide a valid pillar.");

        RuleForEach(x => x.TestSteps)
            .SetValidator(new TestStepCommandRequestValidator());
    }

    private bool BeUniqueTitle(string title)
    {
        var existingTests = _testValidationRepository.GetTestEntitiesWithTitle(title).Result;
        return existingTests.Count == 0;
    }

    private bool BeAValidPillar(string pillar)
    {
        var validPillars = Enum.GetNames(typeof(Pillar)).Select(x => x.ToLower());
        return validPillars.Contains(pillar.ToLower());
    }
}