namespace Teczter.Services.RequestDtos.Executions;

public class CompleteExecutionRequestDto
{
    public int ExecutionId { get; init; }
    public bool HasPassed { get; init; }
    public int? FailedStepId { get; init; }
    public string? FailureReason { get; init; }

}
