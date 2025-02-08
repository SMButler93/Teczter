using Teczter.Domain.ValueObjects;

namespace Teczter.Services.DTOs.Request;

public class TestCommandRequestDto
{
    public string Title { get; set; } = null!;
    public string Description { get; set; } = null!;
    public string Pillar { get; set; } = "UNOWNED";
    public List<TestStepCommandRequestDto> TestSteps { get; set; } = [];
    public List<LinkUrl> LinkUrls { get; set; } = []; 
}
