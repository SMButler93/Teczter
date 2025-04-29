namespace Teczter.Services.RequestDtos.Executions;

public class CompleteExecutionRequestDto
{
    public int ExecutionId { get; }
    public bool HasPassed { get; }
    public int? FailedStepId { get; }
    public string? FailureReason { get; }

}
