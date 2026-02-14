using Teczter.Domain.Entities.interfaces;
using Teczter.Domain.Enums;

namespace Teczter.Domain.Entities;

// ReSharper disable once ClassWithVirtualMembersNeverInherited.Global
public class ExecutionEntity : IAuditableEntity, IHasIntId, ISoftDeleteable
{
    public int Id { get; init; }
    public DateTime CreatedOn { get; private set; } = DateTime.Now;
    public int CreatedById { get; init; }
    public DateTime RevisedOn { get; set; } = DateTime.Now;
    public int RevisedById { get; set; }
    public bool IsDeleted { get; set; }
    public int ExecutionGroupId { get; init; }
    public Guid? AssignedUserId { get; init; }
    public int TestId { get; init; }
    public bool HasPassed => ExecutionState == ExecutionStateType.Pass;
    public int? FailedStepId { get; private set; }
    public string? FailureReason { get; private set; }
    public int? TestedById { get; private set; }
    public List<string> Notes { get; private init; } = [];
    public ExecutionStateType ExecutionState { get; set; } = ExecutionStateType.Untested;
    public virtual required  ExecutionGroupEntity ExecutionGroup { get; init; }
    public virtual TestEntity Test { get; set; } = null!;
    public virtual TestStepEntity? FailedStep { get; set; }
    public byte[] RowVersion { get; set; } = [];

    public void Pass(int userId)
    {
        TestedById = userId;
        ExecutionState = ExecutionStateType.Pass;
        RevisedOn = DateTime.Now;
        //RevisedBy?
    }

    public void Fail(int userId, int testStepId, string failureReason)
    {
        TestedById = userId;
        FailedStepId = testStepId;
        FailureReason = failureReason;
        ExecutionState = ExecutionStateType.Fail;
        RevisedOn = DateTime.Now;
        //RevisedBy?
    }

    public void AddNotes(string note)
    {
        Notes.Add(note);
        RevisedOn = DateTime.Now;
        //RevisedBy?
    }

    public void Reset()
    {
        IsDeleted = false;
        FailedStep = null;
        FailureReason = null;
        TestedById = null;
        ExecutionState = ExecutionStateType.Untested;
        RevisedOn = DateTime.Now;
        //RevisedBy?
    }

    public void Delete()
    {
        var revisedOn = DateTime.Now;
        
        IsDeleted = true;
        RevisedOn = revisedOn; 
        //RevisedBy?
        ExecutionGroup.RevisedOn = revisedOn;
        //ExecutionGroupRevisedBy?
    }

    public ExecutionEntity CloneExecution(ExecutionGroupEntity executionGroup)
    {
        return new ExecutionEntity
        {
            TestId = this.TestId,
            Test = this.Test,
            AssignedUserId = this.AssignedUserId,
            Notes = this.Notes,
            ExecutionState = ExecutionStateType.Untested,
            ExecutionGroup =  executionGroup
            //CreatedBy?
        };
    }
}