namespace Teczter.Services.RequestDtos.TestSteps;

public class UpdateTestStepRequestDto
{
    public int? StepPlacement { get; set; }
    public string? Instructions { get; set; }
    public List<string> Urls { get; set; } = [];
}
