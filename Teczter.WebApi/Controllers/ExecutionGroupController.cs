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
    }
}
