namespace Teczter.Services.RequestDtos.TestSteps;

public class CreateTestStepRequestDto
{
    public int StepPlacement { get; init; }
    public required string Instructions { get; init; }
    public List<string> Urls { get; set; } = [];
}