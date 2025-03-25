using Microsoft.AspNetCore.Mvc;
using Teczter.Services.RequestDtos.Request;
using Teczter.Services.ServiceInterfaces;
using Teczter.WebApi.ResponseDtos;

namespace Teczter.Services.Controllers
{
    [Route("Teczter/[controller]")]
    [ApiController]
    public class ExecutionGroupController : ControllerBase
    {
        private readonly IExecutionGroupService _executionGroupService;

        public ExecutionGroupController(IExecutionGroupService executionGroupService)
        {
            _executionGroupService = executionGroupService;
        }

        [HttpGet]
        public async Task<IActionResult> GetExecutionGroupSearchResults([FromQuery] string? executionGroupName, [FromQuery] string? releaseVersion)
        {
            var executionGroups = await _executionGroupService.GetExecutionGroupSearchResults(executionGroupName, releaseVersion);

            var dtos = executionGroups.Select(x => new ExecutionGroupDto(x)).ToList();

            return Ok(dtos.OrderBy(x => x.CreatedOn).ThenBy(x => x.SoftwareVersionNumber));
        }

        [HttpGet]
        [Route("{id:int}")]
        public async Task<IActionResult> GetExecutionGroup(int id)
        {
            var executionGroup = await _executionGroupService.GetExecutionGroupById(id);

            if (executionGroup == null)
            {
                return NotFound($"Execution group {id} does not exist");
            }

            return Ok(new ExecutionGroupDto(executionGroup));
        }

        [HttpDelete]
        [Route("{id:int}/Delete")]
        public async Task<IActionResult> DeleteExecutionGroup(int id)
        {
            var executionGroup = await _executionGroupService.GetExecutionGroupById(id);

            if (executionGroup == null)
            {
                return NotFound($"Execution group {id} does not exist");
            }

            if (executionGroup.IsComplete)
            {
                return BadRequest("Cannot delete an execution group that has been closed.");
            }

            await _executionGroupService.DeleteExecutionGroup(executionGroup);

            return NoContent();
        }

        [HttpPost]
        [Route("{id:int}/Clone")]
        public async Task<IActionResult> CloneExecutionGroup(int id, [FromQuery] string newExecutionGroupname, [FromQuery] string? softwareVersionNumber)
        {
            var executionGroupToClone = await _executionGroupService.GetExecutionGroupById(id);

            if (executionGroupToClone == null)
            {
                return NotFound($"Execution group {id} does not exist");
            }

            var ValidatedExecutionGroup = await _executionGroupService.CloneExecutionGroup(executionGroupToClone, newExecutionGroupname, softwareVersionNumber);

            if (!ValidatedExecutionGroup.IsValid)
            {
                return BadRequest(ValidatedExecutionGroup.ErrorMessages);
            }

            var dto = new ExecutionGroupDto(ValidatedExecutionGroup.Value!);

            return CreatedAtAction(nameof(GetExecutionGroup), new { dto.Id }, dto);
        }

        [HttpPost]
        [Route("Create")]
        public async Task<IActionResult> CreateExecutionGroup([FromBody] CreateExecutionGroupRequestDto request)
        {
            var validatedExecutionGroup = await _executionGroupService.CreateNewExecutionGroup(request);

            if (!validatedExecutionGroup.IsValid)
            {
                return BadRequest(validatedExecutionGroup.ErrorMessages);
            }

            var dto = new ExecutionGroupDto(validatedExecutionGroup.Value!);

            return CreatedAtAction(nameof(GetExecutionGroup), new { dto.Id }, dto);
        }

        [HttpPost]
        [Route("{id:int}/CreateExecution")]
        public async Task<IActionResult> CreateExecution(int id, [FromBody] CreateExecutionRequestDto request)
        {
            var executionGroup = await _executionGroupService.GetExecutionGroupById(id);

            if (executionGroup == null)
            {
                return NotFound($"Execution group {id} does not exist");
            }

            if (executionGroup.IsComplete)
            {
                return BadRequest("Cannot add new executions to an execution group that has closed.");
            }

            var validatedExecutionGroup = await _executionGroupService.CreateExecution(executionGroup, request);

            if (!validatedExecutionGroup.IsValid)
            {
                return BadRequest(validatedExecutionGroup.ErrorMessages);
            }

            var dto = new ExecutionGroupDto(validatedExecutionGroup.Value!);

            return CreatedAtAction(nameof(GetExecutionGroup), new { dto.Id }, dto);
        }
    }
}
