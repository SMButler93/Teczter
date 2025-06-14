using Microsoft.AspNetCore.Mvc;
using Teczter.Domain.Exceptions;
using Teczter.Services.RequestDtos.Executions;
using Teczter.Services.ServiceInterfaces;
using Teczter.WebApi.ResponseDtos;

namespace Teczter.WebApi.Controllers
{
    [Route("Teczter/[controller]")]
    [ApiController]
    public class ExecutionController(IExecutionService executionService) : ControllerBase
    {
        private readonly IExecutionService _executionService = executionService;

        [HttpGet("{id:int}")]
        public async Task<ActionResult<ExecutionDto>> GetExecutionById(int id)
        {
            var execution = await _executionService.GetExecutionById(id);

            if (execution is null)
            {
                return NotFound($"Execution {id} does not exist.");
            }

            return Ok(new ExecutionDto(execution));
        }

        [HttpPatch("{id:int}")]
        public async Task<ActionResult<ExecutionDto>> CompleteExecution(int id, [FromBody] CompleteExecutionRequestDto request)
        {
            try
            {
                var execution = await _executionService.GetExecutionById(id);

                if (execution is null)
                {
                    return NotFound($"Execution {id} does not exist.");
                }

                var validatedExecution = await _executionService.CompleteExecution(execution, request);

                if (!validatedExecution.IsValid)
                {
                    return BadRequest(validatedExecution.ErrorMessages);
                }

                return Ok(new ExecutionDto(validatedExecution.Value!));
            } 
            catch (TeczterValidationException ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
