using Teczter.Domain.Entities;
using Teczter.Services.RequestDtos.Request;

namespace Teczter.Services.ServiceInterfaces;

public interface IExecutionGroupBuilder
{
    IExecutionGroupBuilder NewInstance();
    IExecutionGroupBuilder UsingContext(ExecutionGroupEntity executionGroup);
    IExecutionGroupBuilder SetName(string name);
    IExecutionGroupBuilder SetSoftwareVersionNumber(string? softwareVersionNumber);
    IExecutionGroupBuilder SetExecutionGroupNotes(List<string>? notes);
    IExecutionGroupBuilder AddExecutions(IEnumerable<CreateExecutionRequestDto> executions);
    IExecutionGroupBuilder AddExecution(CreateExecutionRequestDto execution);
    IExecutionGroupBuilder AddExecutions(IEnumerable<ExecutionEntity> executions);
    IExecutionGroupBuilder AddExecution(ExecutionEntity execution);
    ExecutionGroupEntity Build();
}