using Teczter.Domain.Entities.interfaces;
using Teczter.Domain.Enums;

namespace Teczter.Domain.Entities;

public class ExecutionEntity : IAuditableEntity, IHasIntId, ISoftDeleteable
{
    private ExecutionStateType _executionState = ExecutionStateType.Untested;

    public int Id { get; private set; }
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
    public string? Notes { get; private set; }
    public ExecutionStateType ExecutionState { get; set; }
    public ExecutionGroupEntity ExecutionGroup { get; } = null!;
    public TestEntity Test { get; set; } = null!;
    public TestStepEntity? FailedStep { get; set; }
    public UserEntity? AssignedUser { get; set; }

    public void Pass(int userId)
    {
        TestedById = userId;
        ExecutionState = ExecutionStateType.Pass;
    }

    public void Fail(int userId, int testStepId, string failureReason)
    {
        TestedById = userId;
        FailedStepId = testStepId;
        FailureReason = failureReason;
        ExecutionState = ExecutionStateType.Fail;
    }

    public void AddNotes(string notes) => Notes = notes;

    public ExecutionEntity Retest()
    {
        return new ExecutionEntity
        {
            ExecutionGroupId = this.ExecutionGroupId,
            IsDeleted = this.IsDeleted,
            TestId = this.TestId,
        };
    }

    public void Delete() => IsDeleted = true;

    public ExecutionEntity CloneExecution(int executionGroupId)
    {
        return new ExecutionEntity
        {
            ExecutionGroupId = executionGroupId,
            TestId = TestId,
            Test = Test,
            AssignedUserId = AssignedUserId,
            Notes = Notes,
            ExecutionState = ExecutionStateType.Untested
        };
    }
}