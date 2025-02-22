using Teczter.Domain.Enums;
using Teczter.Domain.ValueObjects;

namespace Teczter.Services.DTOs.Request;

public class TestCommandRequestDto
{
    public string Title { get; set; } = null!;
    public string Description { get; set; } = null!;
    public string OwningPillar { get; set; } = Pillar.Unowned.ToString();
    public List<TestStepCommandRequestDto> TestSteps { get; set; } = [];
    public List<LinkUrl> LinkUrls { get; set; } = []; 
}
