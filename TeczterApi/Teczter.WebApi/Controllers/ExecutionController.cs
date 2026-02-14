using Microsoft.AspNetCore.Mvc;
using Teczter.Services.RequestDtos.Executions;
using Teczter.Services.ServiceInterfaces;
using Teczter.WebApi.ResponseDtos;

namespace Teczter.WebApi.Controllers;

public class ExecutionController(IExecutionService _executionService) : TeczterControllerBase
{
    [HttpGet("{id:int}")]
    public async Task<ActionResult<ExecutionDto>> GetExecutionById(int id, CancellationToken ct)
    {
        var execution = await _executionService.GetExecutionById(id, ct);

        if (execution is null) return NotFound($"Execution {id} does not exist.");
        
        return Ok(new ExecutionDto(execution));
    }

    [HttpPatch("{id:int}")]
    public async Task<ActionResult<ExecutionDto>> CompleteExecution(int id, [FromBody] CompleteExecutionRequestDto request, CancellationToken ct)
    {
        var execution = await _executionService.GetExecutionById(id, ct);
        
        if (execution is null) return NotFound($"Execution {id} does not exist.");
        
        var result = await _executionService.CompleteExecution(execution, request, ct);

        if (!result.IsValid) return BadRequest(result.ErrorMessages);
            
        return Ok(new ExecutionDto(result.Value!));
    }
    
    [HttpDelete("{id:int}")]
    public async Task<ActionResult<ExecutionGroupDto>> DeleteExecution(int id, CancellationToken ct)
    {
        var execution = await _executionService.GetExecutionById(id, ct);
        
        if (execution is null) return NotFound($"Execution {id} does not exist.");
        
        var result = await _executionService.DeleteExecution(execution, ct);

        if (!result.IsValid) return BadRequest(result.ErrorMessages);
            
        return NoContent();
    }
}
