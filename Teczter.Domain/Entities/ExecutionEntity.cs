using Teczter.Domain.Enums;

namespace Teczter.Domain.Entities;

public class ExecutionEntity
{
    private string _executionState = ExecutionStateType.UNTESTED.ToString();

    public Guid Id { get; private set; }
    public Guid ExecutionGroupId { get; init; }
    public int? AssignedUserId { get; set; } = null;
    public bool IsDeleted { get; private set; }
    public DateTime CreatedOn { get; } = DateTime.Now;
    public int CreatedById { get; init; }
    public DateTime RevisedOn { get; set; } = DateTime.Now;
    public int RevisedById { get; set; }
    public Guid TestId { get; set; }
    public bool HasPassed => ExecutionState == ExecutionStateType.PASS.ToString();
    public Guid? FailedStepId { get; private set; } = null;
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

    private bool ValidateTestState(string state)
    {
        var validValues = Enum.GetNames(typeof(ExecutionStateType));

        return validValues.Contains(state.ToUpper());
    }

    public void Pass(int userId)
    {
        TestedById = userId;
        ExecutionState = ExecutionStateType.PASS.ToString();
    }

    public void Fail(int userId, Guid testStepId, string failureReason)
    {
        TestedById = userId;
        FailedStepId = testStepId;
        FailureReason = failureReason;
        ExecutionState = ExecutionStateType.FAIL.ToString();
    }

    public void AddNotes(string notes)
    {
        Notes = notes;
    }

    public ExecutionEntity Retest()
    {
        return new ExecutionEntity
        {
            ExecutionGroupId = this.ExecutionGroupId,
            IsDeleted = this.IsDeleted,
            TestId = this.TestId,
        };
    }

    public void Delete()
    {
        IsDeleted = true;
    }

    public ExecutionEntity CloneExecution(Guid executionGroupId)
    {
        return new ExecutionEntity
        {
            ExecutionGroupId = executionGroupId,
            TestId = TestId,
            Test = Test
        };
    }
}