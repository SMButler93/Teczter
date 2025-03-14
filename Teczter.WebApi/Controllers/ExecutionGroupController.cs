using Microsoft.AspNetCore.Mvc;
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
        public async Task<IActionResult> GetExecutionGroupById(int id)
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
    }
}
