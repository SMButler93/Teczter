using Teczter.Domain.Entities.interfaces;
using Teczter.Domain.Enums;

namespace Teczter.Domain.Entities;

public class ExecutionEntity : IAuditableEntity, IHasIntId, ISoftDeleteable
{
    public int Id { get; set; }
    public DateTime CreatedOn { get; } = DateTime.Now;
    public int CreatedById { get; set; }
    public DateTime RevisedOn { get; set; } = DateTime.Now;
    public int RevisedById { get; set; }
    public bool IsDeleted { get; set; }
    public int ExecutionGroupId { get; init; }
    public Guid? AssignedUserId { get; set; }
    public int TestId { get; set; }
    public bool HasPassed => ExecutionState == ExecutionStateType.Pass;
    public int? FailedStepId { get; private set; }
    public string? FailureReason { get; private set; }
    public int? TestedById { get; private set; }
    public List<string> Notes { get; private set; } = [];
    public ExecutionStateType ExecutionState { get; set; } = ExecutionStateType.Untested;
    public ExecutionGroupEntity ExecutionGroup { get; } = null!;
    public TestEntity Test { get; set; } = null!;
    public TestStepEntity? FailedStep { get; set; }
    public UserEntity? AssignedUser { get; set; }

    public void Pass(int userId)
    {
        TestedById = userId;
        ExecutionState = ExecutionStateType.Pass;
        RevisedOn = DateTime.Now;
    }

    public void Fail(int userId, int testStepId, string failureReason)
    {
        TestedById = userId;
        FailedStepId = testStepId;
        FailureReason = failureReason;
        ExecutionState = ExecutionStateType.Fail;
        RevisedOn = DateTime.Now;
    }

    public void AddNotes(string note)
    {
        Notes.Add(note);
        RevisedOn = DateTime.Now;
    }

    public void Reset()
    {
        RevisedOn = DateTime.Now;
        IsDeleted = false;
        FailedStep = null;
        FailureReason = null;
        TestedById = null;
        ExecutionState = ExecutionStateType.Untested;
    }

    public void Delete()
    {
        IsDeleted = true;
        RevisedOn = DateTime.Now;
    }

    public ExecutionEntity CloneExecution()
    {
        return new ExecutionEntity
        {
            TestId = this.TestId,
            Test = this.Test,
            AssignedUserId = this.AssignedUserId,
            Notes = this.Notes,
            ExecutionState = ExecutionStateType.Untested
        };
    }
}