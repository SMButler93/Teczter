namespace Teczter.Services.DTOs.Request;

public class CreateTestStepRequestDto
{
    public int StepPlacement { get; set; }
    public string Instructions { get; set; } = null!;
    public List<string> Urls { get; set; } = [];
}