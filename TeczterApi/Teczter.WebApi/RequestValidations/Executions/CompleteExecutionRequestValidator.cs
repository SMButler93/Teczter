using FluentValidation;
using Teczter.Services.RequestDtos.Executions;

namespace Teczter.WebApi.RequestValidations.Executions;

public class CompleteExecutionRequestValidator : AbstractValidator<CompleteExecutionRequestDto>
{
    public CompleteExecutionRequestValidator()
    {
        RuleFor(x => x.HasPassed)
            .NotEmpty().WithMessage("A value indicating if the test has passed must be provided.");

        RuleFor(x => x)
            .Must(HasFailedStepWhenFailed).WithMessage("A failed test step must be provided when failing a test.")
            .Must(HasFailureReasonWhenFailed).WithMessage("A failure reason must be provided when failing a test");
    }

    private bool HasFailedStepWhenFailed(CompleteExecutionRequestDto request)
    {
        if (!request.HasPassed)
        {
            return request.FailedStepId.HasValue;
        }

        return !request.FailedStepId.HasValue;
    }

    private bool HasFailureReasonWhenFailed(CompleteExecutionRequestDto request)
    {
        if (!request.HasPassed)
        {
            return !string.IsNullOrEmpty(request.FailureReason);
        }

        return string.IsNullOrEmpty(request.FailureReason);
    }
}
