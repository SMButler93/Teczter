﻿using Teczter.Domain.Entities.interfaces;
using Teczter.Domain.Enums;
using Teczter.Domain.Exceptions;

namespace Teczter.Domain.Entities;

public class ExecutionGroupEntity : IAuditableEntity, IHasIntId, ISoftDeleteable
{
    public int Id { get; set; }
    public DateTime CreatedOn { get; private set; } = DateTime.Now;
    public int CreatedById { get; set; }
    public DateTime RevisedOn { get; set; } = DateTime.Now;
    public int RevisedById { get; set; }
    public bool IsDeleted { get; set; }
    public string ExecutionGroupName { get; set; } = null!;
    public string? SoftwareVersionNumber { get; set; }
    public DateTime? ClosedDate { get; private set; } = null;
    public bool IsComplete => Executions.All(x => x.ExecutionState != ExecutionStateType.Untested);
    public bool IsClosed => ClosedDate.HasValue;
    public List<string> ExecutionGroupNotes { get; set; } = [];
    public int PassedTestPercentage => PassRate();
    public byte[] RowVersion { get; set; } = [];

    public virtual List<ExecutionEntity> Executions { get; set; } = [];

    public void AddExecution(ExecutionEntity execution) => Executions.Add(execution);

    public void Delete()
    {
        if (IsClosed)
        {
            throw new TeczterValidationException("Cannot delete an execution group that " +
                "has been closed.");
        }

        foreach(var execution in Executions)
        {
            execution.Delete();
        }

        IsDeleted = true;
        RevisedOn = DateTime.Now;
    }

    public void DeleteExecution(ExecutionEntity execution)
    {
        execution.Delete();
        Executions.Remove(execution);
        RevisedOn = DateTime.Now;
    }

    public void CloseTestRound() => ClosedDate = DateTime.Now;

    public void AddNote(string note)
    {
        ExecutionGroupNotes.Add(note);
        RevisedOn = DateTime.Now;
    }

    public ExecutionGroupEntity CloneExecutionGroup(string newGroupName, string? versionNumber)
    {
        var executionGroup =  new ExecutionGroupEntity
        {
            ExecutionGroupName = newGroupName,
            SoftwareVersionNumber = versionNumber,
            ExecutionGroupNotes = this.ExecutionGroupNotes,
        };

        foreach (var execution in this.Executions)
        {
            executionGroup.AddExecution(execution.CloneExecution());
        }

        return executionGroup;
    }

    private int PassRate()
    {
        var numberOfTests = Executions.Count;
        var passedTests = Executions.Where(x => x.HasPassed).ToList().Count;

        return (100 / numberOfTests) * passedTests;
    } 
}