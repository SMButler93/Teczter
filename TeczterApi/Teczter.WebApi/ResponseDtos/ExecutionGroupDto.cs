using Teczter.Domain.Entities;

namespace Teczter.WebApi.ResponseDtos;

public class ExecutionGroupDto(ExecutionGroupEntity entity)
{
    public int Id { get; } = entity.Id;
    public DateTime CreatedOn { get; } = entity.CreatedOn;
    public int CreatedById { get; } = entity.CreatedById;
    public DateTime RevisedOn { get; } = entity.RevisedOn;
    public int RevisedById { get; } = entity.RevisedById;
    public bool IsDeleted { get; }  = entity.IsDeleted;
    public string ExecutionGroupName { get; } = entity.ExecutionGroupName;
    public string? SoftwareVersionNumber { get; } = entity.SoftwareVersionNumber;
    public DateTime? ClosedDate { get; } = entity.ClosedDate;
    public bool IsComplete { get; } = entity.IsComplete;
    public bool IsClosed => ClosedDate.HasValue;
    public List<string> ExecutionGroupNotes { get; } = entity.ExecutionGroupNotes;
    public List<ExecutionDto> Executions { get; } = entity.Executions.Select(x => new ExecutionDto(x)).ToList();
}
