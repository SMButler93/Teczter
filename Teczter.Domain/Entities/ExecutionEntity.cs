using Teczter.Domain.Entities.interfaces;
using Teczter.Domain.Enums;

namespace Teczter.Domain.Entities;

public class ExecutionEntity : IAuditableEntity, IHasIntId, ISoftDeleteable
{
    private string _executionState = ExecutionStateType.Untested.ToString();
    private int _id;

    public int Id => _id;
    public DateTime CreatedOn { get; } = DateTime.Now;
    public int CreatedById { get; set; }
    public DateTime RevisedOn { get; set; } = DateTime.Now;
    public int RevisedById { get; set; }
    public bool IsDeleted { get; set; }
    public int ExecutionGroupId { get; init; }
    public Guid? AssignedUserId { get; set; } = null;
    public int TestId { get; set; }
    public bool HasPassed => ExecutionState == ExecutionStateType.Pass.ToString();
    public int? FailedStepId { get; private set; } = null;
    public string? FailureReason { get; private set; } = null;
    public int? TestedById { get; private set; } = null;
    public string? Notes { get; private set; } = null;
    public string ExecutionState
    {
        get
        {
            return _executionState;
        }
        private set
        {
            if (ValidateTestState(value))
            {
                _executionState = value.ToUpper();
            }
        }
    }

    public ExecutionGroupEntity ExecutionGroup { get; } = null!;
    public TestEntity Test { get; set; } = null!;
    public TestStepEntity? FailedStep { get; set; }
    public UserEntity? AssignedUser { get; set; }

    private static bool ValidateTestState(string state)
    {
        var validValues = Enum.GetNames(typeof(ExecutionStateType));

        return validValues.Contains(state.ToUpper());
    }

    public void Pass(int userId)
    {
        TestedById = userId;
        ExecutionState = ExecutionStateType.Pass.ToString();
    }

    public void Fail(int userId, int testStepId, string failureReason)
    {
        TestedById = userId;
        FailedStepId = testStepId;
        FailureReason = failureReason;
        ExecutionState = ExecutionStateType.Fail.ToString();
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
            Test = Test
        };
    }
}