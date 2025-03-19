using Microsoft.AspNetCore.Mvc;
using Teczter.Domain.Exceptions;
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
                return NotFound($"ExecutionGroup {id} does not exist");
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
                return NotFound($"ExecutionGroup {id} does not exist");
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
                return NotFound($"ExecutionGroup {id} does not exist");
            }

            var newExecutiongroup = await _executionGroupService.CloneExecutionGroup(executionGroupToClone, newExecutionGroupname, softwareVersionNumber);

            var dto = new ExecutionGroupDto(newExecutiongroup);

            return CreatedAtAction(nameof(GetExecutionGroup), new { dto.Id }, dto);
        }

        [HttpPost]
        [Route("Create")]
        public async Task<IActionResult> CreateExecutionGroup([FromBody] CreateExecutionGroupRequestDto request)
        {
            var ExecutionGroup = await _executionGroupService.CreateNewExecutionGroup(request);

            var dto = new ExecutionGroupDto(ExecutionGroup);

            return CreatedAtAction(nameof(GetExecutionGroup), new { dto.Id }, dto);
        }
    }
}
