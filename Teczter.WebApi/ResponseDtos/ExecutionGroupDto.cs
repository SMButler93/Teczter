using Teczter.Domain.Entities;
using Teczter.Domain.Enums;

namespace Teczter.WebApi.ResponseDtos;

public class ExecutionGroupDto
{
    public int Id { get; private set; }
    public DateTime CreatedOn { get; }
    public int CreatedById { get; set; }
    public DateTime RevisedOn { get; set; }
    public int RevisedById { get; set; }
    public bool IsDeleted { get; set; }
    public string ExecutionGroupName { get; private set; } = null!;
    public string? SoftwareVersionNumber { get; set; }
    public DateTime? ClosedDate { get; private set; } = null;
    public bool IsComplete { get; set; }
    public bool IsClosed => ClosedDate.HasValue;
    public List<string> ExecutionGroupNotes { get; set; } = [];

    public ExecutionGroupDto(ExecutionGroupEntity entity)
    {

    }
}
