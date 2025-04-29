using Teczter.Domain.Enums;
using Teczter.Services.RequestDtos.TestSteps;

namespace Teczter.Services.RequestDtos.Tests;

public class CreateTestRequestDto
{
    public string Title { get; set; } = null!;
    public string Description { get; set; } = null!;
    public string OwningDepartment { get; set; } = Department.Unowned.ToString();
    public List<CreateTestStepRequestDto> TestSteps { get; set; } = [];
    public List<string> LinkUrls { get; set; } = [];
}
