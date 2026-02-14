namespace Teczter.Services.RequestDtos.TestSteps;

public class UpdateTestStepRequestDto
{
    public int? StepPlacement { get; init; }
    public string? Instructions { get; init; }
    public List<string> Urls { get; init; } = [];
}
