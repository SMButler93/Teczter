namespace Teczter.Services.RequestDtos.Request;

public class CreateExecutionGroupRequestDto
{
    public string ExecutionGroupName { get; set; } = null!;
    public string? SoftwareVersionNumber { get; set; }
    public List<string> ExecutionGroupNotes { get; set; } = [];
    public List<CreateExecutionRequestDto> Executions { get; set; } = [];
}
