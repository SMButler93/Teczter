using FluentValidation;
using Teczter.Adapters.ValidationRepositories.TestValidationRespositories;
using Teczter.Domain.Entities;
using Teczter.Domain.Enums;
using Teczter.Services.ServiceInterfaces;

namespace Teczter.Services.Validation;

public class TestValidator : AbstractValidator<TestEntity>
{
    private readonly ITestValidationRepository _testValidationRepository;
    private readonly IValidator<TestStepEntity> _testStepValidator;

    public TestValidator(
        ITestValidationRepository testValidationRepository,
        IValidator<TestStepEntity> testStepValidator)
    {
        _testValidationRepository = testValidationRepository;
        _testStepValidator = testStepValidator;

        RuleFor(x => x.Title)
            .NotEmpty().WithMessage("A test must have a title.")
            .MustAsync(BeUniqueTitle).WithMessage("A test must have a unique title.");

        RuleFor(x => x.Description)
            .NotEmpty().WithMessage("A test must have a description");

        RuleFor(x => x.OwningPillar)
            .NotEmpty().WithMessage("A test must have an owning pillar")
            .Must(BeAValidPillar).WithMessage("Invalid pillar. Please provide a valid pillar.");

        RuleFor(x => x.CreatedOn)
            .Must(y => y > DateTime.MinValue).WithMessage("Invalid Date. please provide a valid date.")
            .Must(y => y < DateTime.MaxValue).WithMessage("Invalid Date. please provide a valid date.");

        RuleFor(x => x.RevisedOn)
            .Must(y => y > DateTime.MinValue).WithMessage("Invalid Date. please provide a valid date.")
            .Must(y => y < DateTime.MaxValue).WithMessage("Invalid Date. please provide a valid date.");

        RuleFor(x => x.TestSteps)
            .Must(HaveNoDuplicateStepPlacements).WithMessage("An error has occured with the test step placements being duplicated.")
            .Must(HaveNoMissingStepPlacements).WithMessage("An error has occured with test step placements not being sequential.");

        RuleForEach(x => x.TestSteps)
            .SetValidator(_testStepValidator);
    }

    private async Task<bool> BeUniqueTitle(string title, CancellationToken token)
    {
        var existingTests = await _testValidationRepository.GetTestEntitiesWithTitle(title).ConfigureAwait(false);
        return existingTests.Count == 0;
    }

    private bool BeAValidPillar(string pillar)
    {
        var validPillars = Enum.GetNames(typeof(Pillar)).Select(x => x.ToLower());
        return validPillars.Contains(pillar.ToLower());
    }

    private bool HaveNoDuplicateStepPlacements(List<TestStepEntity> testSteps)
    {
        var distinctStepPlacements = testSteps.Select(x => x.StepPlacement).ToHashSet();
        return distinctStepPlacements.Count == testSteps.Count;
    }

    private bool HaveNoMissingStepPlacements(List<TestStepEntity> testSteps)
    {
        var stepPlacements = testSteps.Select(x => x.StepPlacement).ToArray();

        for (var i = 1; i <= stepPlacements.Length; i++)
        {
            if (!stepPlacements.Contains(i))
            {
                return false;
            }
        }

        return true;
    }
}
