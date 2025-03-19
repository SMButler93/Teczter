namespace Teczter.Services.RequestDtos.Request;

public class CreateExecutionRequestDto
{
    public int TestId { get; set; }
    public Guid? AssignedUserId { get; set; }
}
