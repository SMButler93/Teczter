using Teczter.Domain.Entities;

namespace Teczter.WebApi.ResponseDtos;

public class ExecutionDto(ExecutionEntity entity)
{
    public int Id { get; private set; } = entity.Id;
    public DateTime CreatedOn { get; } = entity.CreatedOn;
    public int CreatedById { get; set; } = entity.CreatedById;
    public DateTime RevisedOn { get; set; } = entity.RevisedOn;
    public int RevisedById { get; set; } = entity.RevisedById;
    public bool IsDeleted { get; set; } = entity.IsDeleted;
    public int ExecutionGroupId { get; init; } = entity.ExecutionGroupId;
    public Guid? AssignedUserId { get; set; } = entity.AssignedUserId;
    public int TestId { get; set; } = entity.TestId;
    public bool HasPassed { get; set; } = entity.HasPassed;
    public int? FailedStepId { get; private set; } = entity.FailedStepId;
    public string? FailureReason { get; private set; } = entity.FailureReason;
    public int? TestedById { get; private set; } = entity.TestedById;
    public List<string> Notes { get; private set; } = entity.Notes;
    public string ExecutionState { get; set; } = entity.ExecutionState.ToString();
    public TestDto Test { get; set; } = new TestDto(entity.Test);
    public TestStepDto? FailedStep { get; set; } = entity?.FailedStep is null ? null : new TestStepDto(entity.FailedStep);
}
