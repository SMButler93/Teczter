namespace Teczter.Services.RequestDtos.Request;

public class UpdateTestStepRequestDto
{
    public int? StepPlacement { get; set; }
    public string? Instructions { get; set; }
    public List<string> Urls { get; set; } = [];
}
