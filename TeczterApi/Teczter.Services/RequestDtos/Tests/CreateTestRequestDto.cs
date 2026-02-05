using Teczter.Domain.Enums;
using Teczter.Services.RequestDtos.TestSteps;

namespace Teczter.Services.RequestDtos.Tests;

public class CreateTestRequestDto
{
    public required string Title { get; init; }
    public required string Description { get; init;  }
    public string OwningDepartment { get; init;  } = nameof(Department.Unowned);
    public List<CreateTestStepRequestDto> TestSteps { get; init; } = [];
    public List<string> LinkUrls { get; } = [];
}
