namespace Teczter.Services.RequestDtos.Request;

public class UpdateTestRequestDto
{
    public string Title { get; set; } = null!;
    public string Description { get; set; } = null!;
    public string OwningDepartment { get; set; } = null!;
}
