using Teczter.Domain.ValueObjects;

namespace Teczter.WebApi.DTOs.Request;

public class CreateTestRequestDto
{
    public string Title { get; set; } = null!;
    public string Description { get; set; } = null!;
    public string Pillar { get; set; } = "UNOWNED";
}
