using Microsoft.AspNetCore.Mvc;
using Teczter.Services.ServiceInterfaces;
using Teczter.WebApi.ResponseDtos;

namespace Teczter.WebApi.Controllers
{
    [Route("Teczter/[controller]")]
    [ApiController]
    public class ExecutionController : ControllerBase
    {
        private readonly IExecutionService _executionService;

        public ExecutionController(IExecutionService executionService)
        {
            _executionService = executionService;
        }

        [HttpGet]
        [Route("Execution/{id:int}")]
        public async Task<IActionResult> GetExecutionById(int id)
        {
            var execution = await _executionService.GetExecutionById(id);

            if (execution == null)
            {
                return NotFound($"Execution {id} does not exist.");
            }

            return Ok(new ExecutionDto(execution));
        }
    }
}
