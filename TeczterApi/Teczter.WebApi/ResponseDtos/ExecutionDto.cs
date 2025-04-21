using Teczter.Domain.Entities;

namespace Teczter.WebApi.ResponseDtos;

public class ExecutionDto
{
    public int Id { get; private set; }
    public string CreatedOn { get; }
    public int CreatedById { get; set; }
    public string RevisedOn { get; set; }
    public int RevisedById { get; set; }
    public bool IsDeleted { get; set; }
    public int ExecutionGroupId { get; init; }
    public Guid? AssignedUserId { get; set; }
    public int TestId { get; set; }
    public bool HasPassed { get; set; }
    public int? FailedStepId { get; private set; }
    public string? FailureReason { get; private set; }
    public int? TestedById { get; private set; }
    public List<string> Notes { get; private set; }
    public string ExecutionState { get; set; }
    public TestDto Test { get; set; } = null!;
    public TestStepDto? FailedStep { get; set; }

    public ExecutionDto(ExecutionEntity entity)
    {
        Id = entity.Id;
        CreatedOn = entity.CreatedOn.ToString();
        CreatedById = entity.CreatedById;
        RevisedOn = entity.RevisedOn.ToString();
        RevisedById = entity.RevisedById;
        IsDeleted = entity.IsDeleted;
        ExecutionGroupId = entity.ExecutionGroupId;
        AssignedUserId = entity.AssignedUserId;
        TestId = entity.TestId;
        HasPassed = entity.HasPassed;
        FailedStepId = entity.FailedStepId;
        FailureReason = entity.FailureReason;
        TestedById = entity.TestedById;
        Notes = entity.Notes;
        ExecutionState = entity.ExecutionState.ToString();
        Test = new TestDto(entity.Test);
        FailedStep = entity?.FailedStep == null ? null : new TestStepDto(entity.FailedStep);
    }
}
