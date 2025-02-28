using Teczter.Domain.Enums;

namespace Teczter.Domain.Entities;

public class ExecutionGroupEntity
{
    public Guid Id { get; private set; }
    public bool IsDeleted { get; set; }
    public DateTime CreatedOn { get; } = DateTime.Now;
    public int CreatedById { get; }
    public DateTime RevisedOn { get; set; } = DateTime.Now;
    public int RevisedById { get; set; }
    public string ExecutionGroupName { get; private set; } = null!;
    public DateTime? ClosedDate { get; private set; } = null;
    public bool IsComplete => Executions.All(x => x.ExecutionState != ExecutionStateType.Untested.ToString());
    public bool IsClosed => ClosedDate.HasValue;
    public List<string> ExecutionGroupNotes { get; set; } = [];

    public List<ExecutionEntity> Executions { get; set; } = [];

    public void AddExecution(ExecutionEntity execution) => Executions.Add(execution);

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