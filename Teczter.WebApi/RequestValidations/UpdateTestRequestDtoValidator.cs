using FluentValidation;
using Teczter.Adapters.ValidationRepositories.TestValidationRespositories;
using Teczter.Services.RequestDtos.Request;
using Teczter.Services.Validation.ValidationRules;

namespace Teczter.WebApi.RequestValidations;

public class UpdateTestRequestDtoValidator : AbstractValidator<UpdateTestRequestDto>
{
    private readonly ITestValidationRepository _testValidationRepository;
    public UpdateTestRequestDtoValidator(ITestValidationRepository testValidationRepository)
    {
        _testValidationRepository = testValidationRepository;

        RuleFor(x => x.Title)
            .NotEmpty().WithMessage("A test must have a title.")
            .Must(title => TestValidationRules.BeUniqueTitle(title, _testValidationRepository, new CancellationToken()).Result)
            .WithMessage(dto => $"A test with the title '{dto.Title}' already exists. A title must be unique.");

        RuleFor(x => x.Description)
            .NotEmpty().WithMessage("A test must have a description");

        RuleFor(x => x.OwningDepartment)
            .NotEmpty().WithMessage("A test must have an owning department")
            .Must(TestValidationRules.BeAValidDepartment).WithMessage("Invalid department. Please provide a valid department.");
    }
}