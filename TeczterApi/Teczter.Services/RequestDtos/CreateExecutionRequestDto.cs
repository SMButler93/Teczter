namespace Teczter.Services.RequestDtos;

public class CreateExecutionRequestDto
{
    public int TestId { get; set; }
    public Guid? AssignedUserId { get; set; }
}
