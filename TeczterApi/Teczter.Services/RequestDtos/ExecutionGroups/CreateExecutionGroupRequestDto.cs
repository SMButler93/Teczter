using Teczter.Services.RequestDtos.Executions;

namespace Teczter.Services.RequestDtos.ExecutionGroups;

public class CreateExecutionGroupRequestDto
{
    public required string ExecutionGroupName { get; init; }
    public string? SoftwareVersionNumber { get; init; }
    public List<string> ExecutionGroupNotes { get; init; } = [];
    public List<CreateExecutionRequestDto> Executions { get; init; } = [];
}
