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

        [HttpGet]
        [Route("{id:int}")]
        public async Task<IActionResult> GetExecutionById(int id)
        {
            var execution = await _executionService.GetExecutionById(id);

            if (execution is null)
            {
                return NotFound($"Execution {id} does not exist.");
            }

            return Ok(new ExecutionDto(execution));
        }

        [HttpPut]
        [Route("CompleteExecution")]
        public async Task<IActionResult> CompleteExecution([FromBody] CompleteExecutionRequestDto request)
        {
            try
            {
                var validatedExecution = await _executionService.CompleteExecution(request);

                if (!validatedExecution.IsValid)
                {
                    return BadRequest(validatedExecution.ErrorMessages);
                }

                return Ok(validatedExecution.Value);
            } catch (TeczterValidationException ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
