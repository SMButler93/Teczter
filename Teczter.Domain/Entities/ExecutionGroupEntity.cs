﻿using Teczter.Domain.Entities.interfaces;
using Teczter.Domain.Enums;

namespace Teczter.Domain.Entities;

public class ExecutionGroupEntity : IAuditableEntity, IHasIntId, ISoftDeleteable
{
    public int Id { get; private set; }
    public DateTime CreatedOn { get; } = DateTime.Now;
    public int CreatedById { get; set; }
    public DateTime RevisedOn { get; set; } = DateTime.Now;
    public int RevisedById { get; set; }
    public bool IsDeleted { get; set; }
    public string ExecutionGroupName { get; private set; } = null!;
    public string? SoftwareVersionNumber { get; set; }
    public DateTime? ClosedDate { get; private set; } = null;
    public bool IsComplete => Executions.All(x => x.ExecutionState != ExecutionStateType.Untested);
    public bool IsClosed => ClosedDate.HasValue;
    public List<string> ExecutionGroupNotes { get; set; } = [];

    public List<ExecutionEntity> Executions { get; set; } = [];

    public void AddExecution(ExecutionEntity execution) => Executions.Add(execution);

    public void Delete()
    {
        foreach(var execution in Executions)
        {
            DeleteExecution(execution);
        }

        IsDeleted = true;
    }

    public void DeleteExecution(ExecutionEntity execution) => execution.Delete();

    public void CloseTestRound() => ClosedDate = DateTime.Now;

    public void AddNotes(string note)
    {
        ExecutionGroupNotes.Add(note);
    }

    public ExecutionGroupEntity CloneExecutionGroup(string newGroupName)
    {
        return new ExecutionGroupEntity
        {
            ExecutionGroupName = newGroupName,
            Executions = Executions.Select(x => x.CloneExecution(Id)).ToList(),
            ExecutionGroupNotes = ExecutionGroupNotes
        };
    }
}