using Teczter.Domain.Enums;

namespace Teczter.Services.DTOs.Request;

public class CreateTestRequestDto
{
    public string Title { get; set; } = null!;
    public string Description { get; set; } = null!;
    public string OwningDepartment { get; set; } = Department.Unowned.ToString();
    public List<TestStepCommandRequestDto> TestSteps { get; set; } = [];
    public List<string> LinkUrls { get; set; } = [];
}
