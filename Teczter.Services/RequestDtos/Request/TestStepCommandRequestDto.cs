using Teczter.Domain.ValueObjects;

namespace Teczter.Services.DTOs.Request;

public class TestStepCommandRequestDto
{
    public int StepPlacement { get; set; }
    public string Instructions { get; set; } = null!;
    public List<LinkUrl> LinkUrls { get; set; } = [];
}