namespace Teczter.Services.RequestDtos.Executions;

public class CreateExecutionRequestDto
{
    public int TestId { get; set; }
    public Guid? AssignedUserId { get; set; }
}
