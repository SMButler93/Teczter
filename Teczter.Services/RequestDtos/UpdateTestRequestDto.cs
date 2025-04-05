using Teczter.Domain.Enums;

namespace Teczter.Services.RequestDtos;

public class UpdateTestRequestDto
{
    public string? Title { get; set; }
    public string? Description { get; set; }
    public string? OwningDepartment { get; set; }
}
