namespace Teczter.Services.RequestDtos.Executions;

public class CompleteExecutionRequestDto
{
    public bool HasPassed { get; init; }
    public int? FailedStepId { get; init; }
    public string? FailureReason { get; init; }

}
