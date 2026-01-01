using Microsoft.AspNetCore.Mvc;
using Teczter.Services.RequestDtos.ExecutionGroups;
using Teczter.Services.RequestDtos.Executions;
using Teczter.Services.ServiceInterfaces;
using Teczter.WebApi.ResponseDtos;

namespace Teczter.WebApi.Controllers;

[Route("Teczter/[controller]")]
[ApiController]
public class ExecutionGroupsController(IExecutionGroupService executionGroupService, IExecutionService executionService) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<ExecutionGroupDto>> GetExecutionGroupSearchResults(
        [FromQuery] string? executionGroupName, 
        [FromQuery] string? releaseVersion, 
        CancellationToken ct, 
        [FromQuery] int pageNumber = 1)
    {
        var executionGroups = await executionGroupService.GetExecutionGroupSearchResults(pageNumber, executionGroupName, releaseVersion, ct);

        var executionGroupDtos = executionGroups.Select(x => new ExecutionGroupDto(x)).ToList();

        return Ok(executionGroupDtos);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<ExecutionGroupDto>> GetExecutionGroup(int id, CancellationToken ct)
    {
        var executionGroup = await executionGroupService.GetExecutionGroupById(id, ct);

        if (executionGroup is null)
        {
            return NotFound($"Execution group {id} does not exist");
        }

        return Ok(new ExecutionGroupDto(executionGroup));
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteExecutionGroup(int id, CancellationToken ct)
    {
        var executionGroup = await executionGroupService.GetExecutionGroupById(id, ct);

        if (executionGroup is null)
        {
            return NotFound($"Execution group {id} does not exist");
        }

        if (executionGroup.IsClosed)
        {
            return BadRequest("Cannot delete an execution group that has been closed.");
        }

        var result = await executionGroupService.DeleteExecutionGroup(executionGroup, ct);

        if (!result.IsValid)
        {
            return BadRequest(result.ErrorMessages);
        }

        return NoContent();
    }

    [HttpPost("{id:int}/clone")]
    public async Task<ActionResult<ExecutionGroupDto>> CloneExecutionGroup(
        int id, 
        [FromQuery] string newExecutionGroupName, 
        [FromQuery] string? softwareVersionNumber,
        CancellationToken ct)
    {
        var executionGroupToClone = await executionGroupService.GetExecutionGroupById(id, ct);

        if (executionGroupToClone is null)
        {
            return NotFound($"Execution group {id} does not exist");
        }

        var result = await executionGroupService.CloneExecutionGroup(executionGroupToClone, newExecutionGroupName, softwareVersionNumber, ct);

        if (!result.IsValid)
        {
            return BadRequest(result.ErrorMessages);
        }

        var dto = new ExecutionGroupDto(result.Value!);

        return CreatedAtAction(nameof(GetExecutionGroup), new { dto.Id }, dto);
    }

    [HttpPost]
    public async Task<ActionResult<ExecutionGroupDto>> CreateExecutionGroup([FromBody] CreateExecutionGroupRequestDto request, CancellationToken ct)
    {
        var result = await executionGroupService.CreateNewExecutionGroup(request, ct);

        if (!result.IsValid)
        {
            return BadRequest(result.ErrorMessages);
        }

        var dto = new ExecutionGroupDto(result.Value!);

        return CreatedAtAction(nameof(GetExecutionGroup), new { dto.Id }, dto);
    }

    [HttpPost("{id:int}/Executions")]
    public async Task<ActionResult<ExecutionGroupDto>> CreateExecution(int id, [FromBody] CreateExecutionRequestDto request, CancellationToken ct)
    {
        var executionGroup = await executionGroupService.GetExecutionGroupById(id, ct);

        if (executionGroup is null)
        {
            return NotFound($"Execution group {id} does not exist");
        }

        if (executionGroup.IsClosed)
        {
            return BadRequest("Cannot add new executions to an execution group that has closed.");
        }

        var result = await executionGroupService.CreateExecution(executionGroup, request, ct);

        if (!result.IsValid)
        {
            return BadRequest(result.ErrorMessages);
        }

        var dto = new ExecutionGroupDto(result.Value!);

        return CreatedAtAction(nameof(GetExecutionGroup), new { dto.Id }, dto);
    }

    [HttpDelete("{groupId:int}/Executions/{executionId:int}")]
    public async Task<ActionResult<ExecutionGroupDto>> DeleteExecution(int groupId, int executionId, CancellationToken ct)
    {
        var executionGroup = await executionGroupService.GetExecutionGroupById(groupId, ct);

        if (executionGroup is null)
        {
            return NotFound($"Execution group {groupId} does not exist.");
        }

        if (executionGroup.IsClosed)
        {
            return BadRequest($"Cannot delete executions from an execution group that has been closed.");
        }

        var result = await executionGroupService.DeleteExecution(executionGroup, executionId, ct);

        if (!result.IsValid)
        {
            return BadRequest(result.ErrorMessages);
        }

        return Ok(new ExecutionGroupDto(executionGroup));
    }

    [HttpGet("{groupId:int}/Executions/{executionId:int}")]
    public async Task<ActionResult<ExecutionDto>> GetExecutionById(int groupId, int executionId, CancellationToken ct)
    {
        var executionGroup = await executionGroupService.GetExecutionGroupById(groupId, ct);

        if (executionGroup is null)
        {
            return BadRequest($"Execution group {groupId} does not exist.");
        }

        var execution = executionGroup.Executions.SingleOrDefault(x => x.Id == executionId);

        if (execution is null)
        {
            return BadRequest($"Execution {executionId} does not exist in execution group {groupId}");
        }

        return Ok(new ExecutionDto(execution));
    }

    [HttpPatch("{groupId:int})/Executions/{executionId:int}")]
    public async Task<ActionResult<ExecutionDto>> CompleteExecution(int groupId, int executionId, [FromBody] CompleteExecutionRequestDto request, CancellationToken ct)
    {
        var executionGroup = await executionGroupService.GetExecutionGroupById(groupId, ct);

        if (executionGroup is null)
        {
            return BadRequest($"Execution group {groupId} does not exist.");
        }

        var execution = executionGroup.Executions.SingleOrDefault(x => x.Id == executionId);

        if (execution is null)
        { 
            return BadRequest($"Execution {executionId} does not exist in execution group {groupId}");
        }

        var result = await executionService.CompleteExecution(execution, request, ct);

        if (!result.IsValid)
        {
            return BadRequest(result.ErrorMessages);
        }

        return Ok(new ExecutionDto(result.Value!));
    }
}