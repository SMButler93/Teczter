using Teczter.Domain;
using Teczter.Domain.Entities;
using Teczter.Services.RequestDtos.Executions;
using Teczter.Services.ServiceInterfaces;

namespace Teczter.Services.ComposersAndBuilders;

public class ExecutionGroupComposer : IExecutionGroupComposer
{
    private ExecutionGroupEntity _executionGroup = new();
    private List<string> Errors = [];

    public IExecutionGroupComposer AddExecution(CreateExecutionRequestDto execution)
    {
        _executionGroup.AddExecution(
            new ExecutionEntity
            {
                ExecutionGroupId = _executionGroup.Id,
                TestId = execution.TestId,
                AssignedUserId = execution.AssignedUserId
            });

        return this;
    }

    public IExecutionGroupComposer AddExecution(ExecutionEntity execution)
    {
        _executionGroup.AddExecution(execution);
        return this;
    }

    public IExecutionGroupComposer AddExecutions(IEnumerable<CreateExecutionRequestDto> executions)
    {
        foreach (var execution in executions)
        {
            AddExecution(execution);
        }

        return this;
    }

    public IExecutionGroupComposer AddExecutions(IEnumerable<ExecutionEntity> executions)
    {
        foreach (var execution in executions)
        {
            _executionGroup.AddExecution(execution);
        }

        return this;
    }

    public TeczterValidationResult<ExecutionGroupEntity> Build()
    {
        if (Errors.Any())
        {
            return TeczterValidationResult<ExecutionGroupEntity>.Fail(Errors.ToArray());
        }

        return TeczterValidationResult<ExecutionGroupEntity>.Succeed(_executionGroup);
    }

    public IExecutionGroupComposer SetExecutionGroupNotes(List<string>? notes)
    {
        _executionGroup.ExecutionGroupNotes = notes ?? [];
        return this;
    }

    public IExecutionGroupComposer SetName(string name)
    {
        _executionGroup.ExecutionGroupName = name;
        return this;
    }

    public IExecutionGroupComposer SetSoftwareVersionNumber(string? softwareVersionNumber)
    {
        _executionGroup.SoftwareVersionNumber = softwareVersionNumber;
        return this;
    }

    public IExecutionGroupComposer UsingContext(ExecutionGroupEntity executionGroup)
    {
        _executionGroup = executionGroup;
        SetRevisionDetails();

        return this;
    }

    private void SetRevisionDetails()
    {
        _executionGroup.RevisedOn = DateTime.Now;
    }
}
