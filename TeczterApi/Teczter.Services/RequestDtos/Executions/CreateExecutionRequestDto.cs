namespace Teczter.Services.RequestDtos.Executions;

public class CreateExecutionRequestDto
{
    public int TestId { get; init; }
    public Guid? AssignedUserId { get; init; }
}
