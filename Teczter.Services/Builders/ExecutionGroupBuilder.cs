using Teczter.Domain.Entities;
using Teczter.Services.RequestDtos;
using Teczter.Services.ServiceInterfaces;

namespace Teczter.Services.Builders;

internal class ExecutionGroupBuilder : IExecutionGroupBuilder
{
    private ExecutionGroupEntity _executionGroup = null!;

    public IExecutionGroupBuilder AddExecution(CreateExecutionRequestDto execution)
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

    public IExecutionGroupBuilder AddExecution(ExecutionEntity execution)
    {
        _executionGroup.AddExecution(execution);
        return this;
    }

    public IExecutionGroupBuilder AddExecutions(IEnumerable<CreateExecutionRequestDto> executions)
    {
        foreach (var execution in executions)
        {
            AddExecution(execution);
        }

        return this;
    }

    public IExecutionGroupBuilder AddExecutions(IEnumerable<ExecutionEntity> executions)
    {
        foreach (var execution in executions)
        {
            _executionGroup.AddExecution(execution);
        }

        return this;
    }

    public ExecutionGroupEntity Build()
    {
        return _executionGroup;
    }

    public IExecutionGroupBuilder NewInstance()
    {
        _executionGroup = new();
        return this;
    }

    public IExecutionGroupBuilder SetExecutionGroupNotes(List<string>? notes)
    {
        _executionGroup.ExecutionGroupNotes = notes ?? [];
        return this;
    }

    public IExecutionGroupBuilder SetName(string name)
    {
        _executionGroup.ExecutionGroupName = name;
        return this;
    }

    public IExecutionGroupBuilder SetSoftwareVersionNumber(string? softwareVersionNumber)
    {
        _executionGroup.SoftwareVersionNumber = softwareVersionNumber;
        return this;
    }

    public IExecutionGroupBuilder UsingContext(ExecutionGroupEntity executionGroup)
    {
        _executionGroup = executionGroup;
        SetRevisonDetails();

        return this;
    }

    private void SetRevisonDetails()
    {
        _executionGroup.RevisedOn = DateTime.Now;
    }
}
