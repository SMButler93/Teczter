using Teczter.Domain.Entities;

namespace Teczter.WebApi.ResponseDtos;

public class ExecutionGroupDto
{
    public int Id { get; }
    public string CreatedOn { get; }
    public int CreatedById { get; }
    public string RevisedOn { get; }
    public int RevisedById { get; }
    public bool IsDeleted { get; }
    public string ExecutionGroupName { get; }
    public string? SoftwareVersionNumber { get; }
    public DateTime? ClosedDate { get; }
    public bool IsComplete { get; }
    public bool IsClosed => ClosedDate.HasValue;
    public List<string> ExecutionGroupNotes { get; }
    public List<ExecutionDto> Executions { get; }

    public ExecutionGroupDto(ExecutionGroupEntity entity)
    {
        Id = entity.Id;
        CreatedOn = entity.CreatedOn.ToString();
        CreatedById = entity.CreatedById;
        RevisedOn = entity.RevisedOn.ToString();
        RevisedById = entity.RevisedById;
        IsDeleted = entity.IsDeleted;
        ExecutionGroupName = entity.ExecutionGroupName;
        SoftwareVersionNumber = entity.SoftwareVersionNumber;
        ClosedDate = entity.ClosedDate;
        IsComplete = entity.IsComplete;
        ExecutionGroupNotes = entity.ExecutionGroupNotes;
        Executions = entity.Executions.Select(x => new ExecutionDto(x)).ToList();
    }
}
