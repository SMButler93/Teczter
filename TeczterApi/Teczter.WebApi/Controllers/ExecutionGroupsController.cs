using Microsoft.AspNetCore.Mvc;
using Teczter.Domain.Exceptions;
using Teczter.Services.RequestDtos.ExecutionGroups;
using Teczter.Services.RequestDtos.Executions;
using Teczter.Services.ServiceInterfaces;
using Teczter.WebApi.ResponseDtos;

namespace Teczter.WebApi.Controllers;

[Route("Teczter/[controller]")]
[ApiController]
public class ExecutionGroupsController(IExecutionGroupService executionGroupService, IExecutionService executionService) : ControllerBase
{
    private readonly IExecutionGroupService _executionGroupService = executionGroupService;
    private readonly IExecutionService _exececutionService = executionService;

    [HttpGet]
    public async Task<ActionResult<ExecutionGroupDto>> GetExecutionGroupSearchResults([FromQuery] string? executionGroupName, [FromQuery] string? releaseVersion, [FromQuery] int pageNumber = 1)
    {
        var executionGroups = await _executionGroupService.GetExecutionGroupSearchResults(pageNumber, executionGroupName, releaseVersion);

        var executionGroupDtos = executionGroups.Select(x => new ExecutionGroupDto(x)).ToList();

        return Ok(executionGroupDtos);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<ExecutionGroupDto>> GetExecutionGroup(int id)
    {
        var executionGroup = await _executionGroupService.GetExecutionGroupById(id);

        if (executionGroup is null)
        {
            return NotFound($"Execution group {id} does not exist");
        }

        return Ok(new ExecutionGroupDto(executionGroup));
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteExecutionGroup(int id)
    {
        var executionGroup = await _executionGroupService.GetExecutionGroupById(id);

        if (executionGroup is null)
        {
            return NotFound($"Execution group {id} does not exist");
        }

        if (executionGroup.IsClosed)
        {
            return BadRequest("Cannot delete an execution group that has been closed.");
        }

        try
        {
            await _executionGroupService.DeleteExecutionGroup(executionGroup);

            return NoContent();
        }
        catch (TeczterValidationException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPost("{id:int}/clones")]
    public async Task<ActionResult<ExecutionGroupDto>> CloneExecutionGroup(int id, [FromQuery] string newExecutionGroupname, [FromQuery] string? softwareVersionNumber)
    {
        var executionGroupToClone = await _executionGroupService.GetExecutionGroupById(id);

        if (executionGroupToClone is null)
        {
            return NotFound($"Execution group {id} does not exist");
        }

        var result = await _executionGroupService.CloneExecutionGroup(executionGroupToClone, newExecutionGroupname, softwareVersionNumber);

        if (!result.IsValid)
        {
            return BadRequest(result.ErrorMessages);
        }

        var dto = new ExecutionGroupDto(result.Value!);

        return CreatedAtAction(nameof(GetExecutionGroup), new { dto.Id }, dto);
    }

    [HttpPost]
    public async Task<ActionResult<ExecutionGroupDto>> CreateExecutionGroup([FromBody] CreateExecutionGroupRequestDto request)
    {
        var result = await _executionGroupService.CreateNewExecutionGroup(request);

        if (!result.IsValid)
        {
            return BadRequest(result.ErrorMessages);
        }

        var dto = new ExecutionGroupDto(result.Value!);

        return CreatedAtAction(nameof(GetExecutionGroup), new { dto.Id }, dto);
    }

    [HttpPost("{id:int}/Executions")]
    public async Task<ActionResult<ExecutionGroupDto>> CreateExecution(int id, [FromBody] CreateExecutionRequestDto request)
    {
        var executionGroup = await _executionGroupService.GetExecutionGroupById(id);

        if (executionGroup is null)
        {
            return NotFound($"Execution group {id} does not exist");
        }

        if (executionGroup.IsClosed)
        {
            return BadRequest("Cannot add new executions to an execution group that has closed.");
        }

        var result = await _executionGroupService.CreateExecution(executionGroup, request);

        if (!result.IsValid)
        {
            return BadRequest(result.ErrorMessages);
        }

        var dto = new ExecutionGroupDto(result.Value!);

        return CreatedAtAction(nameof(GetExecutionGroup), new { dto.Id }, dto);
    }

    [HttpDelete("{groupId:int}/Executions/{executionId:int}")]
    public async Task<ActionResult<ExecutionGroupDto>> DeleteExecution(int groupId, int executionId)
    {
        var executionGroup = await _executionGroupService.GetExecutionGroupById(groupId);

        if (executionGroup is null)
        {
            return NotFound($"Execution group {groupId} does not exist.");
        }

        if (executionGroup.IsClosed)
        {
            return BadRequest($"Cannot delete executions from an execution group that has been closed.");
        }

        try
        {
            await _executionGroupService.DeleteExecution(executionGroup, executionId);

            return Ok(new ExecutionGroupDto(executionGroup));
        }
        catch (TeczterValidationException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpGet("{groupId:int}/Executions/{executionId:int}")]
    public async Task<ActionResult<ExecutionDto>> GetExecutionById(int groupId, int executionId)
    {
        var executionGroup = await _executionGroupService.GetExecutionGroupById(groupId);

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

    [HttpPatch("{groupId: int})/Executions/{executionId:int}")]
    public async Task<ActionResult<ExecutionDto>> CompleteExecution(int groupId, int executionId, [FromBody] CompleteExecutionRequestDto request)
    {
        try
        {
            var executionGroup = await _executionGroupService.GetExecutionGroupById(groupId);

            if (executionGroup is null)
            {
                return BadRequest($"Execution group {groupId} does not exist.");
            }

            var execution = executionGroup.Executions.SingleOrDefault(x => x.Id == executionId);

            if (execution is null)
            {
                return BadRequest($"Execution {executionId} does not exist in execution group {groupId}");
            }

            var result = await _exececutionService.CompleteExecution(execution, request);

            if (!result.IsValid)
            {
                return BadRequest(result.ErrorMessages);
            }

            return Ok(new ExecutionDto(result.Value!));
        }
        catch (TeczterValidationException ex)
        {
            return BadRequest(ex.Message);
        }
    }
}
