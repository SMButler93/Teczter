using Microsoft.AspNetCore.Mvc;
using Teczter.Services.RequestDtos.ExecutionGroups;
using Teczter.Services.RequestDtos.Executions;
using Teczter.Services.ServiceInterfaces;
using Teczter.WebApi.ResponseDtos;

namespace Teczter.WebApi.Controllers;

public class ExecutionGroupController(IExecutionGroupService _executionGroupService) : TeczterControllerBase
{
    [HttpGet]
    public async Task<ActionResult<ExecutionGroupDto>> GetExecutionGroupSearchResults(
        [FromQuery] string? executionGroupName, 
        [FromQuery] string? releaseVersion, 
        CancellationToken ct, 
        [FromQuery] int pageNumber = 1)
    {
        var executionGroups = await _executionGroupService.GetExecutionGroupSearchResults(pageNumber, executionGroupName, releaseVersion, ct);

        return Ok(executionGroups.Select(x => new ExecutionGroupDto(x)).ToList());
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<ExecutionGroupDto>> GetExecutionGroup(int id, CancellationToken ct)
    {
        var executionGroup = await _executionGroupService.GetExecutionGroupById(id, ct);

        if (executionGroup is null) return NotFound($"Execution group {id} does not exist");
            
        return Ok(new ExecutionGroupDto(executionGroup));
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteExecutionGroup(int id, CancellationToken ct)
    {
        var executionGroup = await _executionGroupService.GetExecutionGroupById(id, ct);

        if (executionGroup is null) return NotFound($"Execution group {id} does not exist");

        var result = await _executionGroupService.DeleteExecutionGroup(executionGroup, ct);

        if (!result.IsValid) return BadRequest(result.ErrorMessages);

        return NoContent();
    }

    [HttpPost("{id:int}/clone")]
    public async Task<ActionResult<ExecutionGroupDto>> CloneExecutionGroup(
        int id, 
        [FromQuery] string newExecutionGroupName, 
        [FromQuery] string? softwareVersionNumber,
        CancellationToken ct)
    {
        var executionGroupToClone = await _executionGroupService.GetExecutionGroupById(id, ct);

        if (executionGroupToClone is null) return NotFound($"Execution group {id} does not exist");

        var result = await _executionGroupService.CloneExecutionGroup(executionGroupToClone, newExecutionGroupName, softwareVersionNumber, ct);

        if (!result.IsValid) return BadRequest(result.ErrorMessages);

        var dto = new ExecutionGroupDto(result.Value!);

        return CreatedAtAction(nameof(GetExecutionGroup), new { dto.Id }, dto);
    }

    [HttpPost]
    public async Task<ActionResult<ExecutionGroupDto>> CreateExecutionGroup([FromBody] CreateExecutionGroupRequestDto request, CancellationToken ct)
    {
        var result = await _executionGroupService.CreateNewExecutionGroup(request, ct);

        if (!result.IsValid) return BadRequest(result.ErrorMessages);

        var dto = new ExecutionGroupDto(result.Value!);

        return CreatedAtAction(nameof(GetExecutionGroup), new { dto.Id }, dto);
    }
    
    [HttpPost("{groupId:int}/CreateExecution")]
    public async Task<ActionResult<ExecutionGroupDto>> CreateExecution(int groupId, [FromBody] CreateExecutionRequestDto request, CancellationToken ct)
    {
        var result = await _executionGroupService.CreateExecution(groupId, request, ct);

        if (!result.IsValid) return BadRequest(result.ErrorMessages);

        var dto = new ExecutionGroupDto(result.Value!);

        return CreatedAtAction(nameof(GetExecutionGroup), new { dto.Id }, dto);
    }
}