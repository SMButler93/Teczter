using Teczter.Domain.Entities;
using Teczter.Services.RequestDtos;

namespace Teczter.Services.ServiceInterfaces;

public interface IExecutionGroupComposer
{
    IExecutionGroupComposer NewInstance();
    IExecutionGroupComposer UsingContext(ExecutionGroupEntity executionGroup);
    IExecutionGroupComposer SetName(string name);
    IExecutionGroupComposer SetSoftwareVersionNumber(string? softwareVersionNumber);
    IExecutionGroupComposer SetExecutionGroupNotes(List<string>? notes);
    IExecutionGroupComposer AddExecutions(IEnumerable<CreateExecutionRequestDto> executions);
    IExecutionGroupComposer AddExecution(CreateExecutionRequestDto execution);
    IExecutionGroupComposer AddExecutions(IEnumerable<ExecutionEntity> executions);
    IExecutionGroupComposer AddExecution(ExecutionEntity execution);
    ExecutionGroupEntity Build();
}