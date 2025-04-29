namespace Teczter.Services.RequestDtos.TestSteps;

public class CreateTestStepRequestDto
{
    public int StepPlacement { get; set; }
    public string Instructions { get; set; } = null!;
    public List<string> Urls { get; set; } = [];
}