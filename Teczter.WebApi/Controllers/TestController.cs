using Microsoft.AspNetCore.Mvc;
using Teczter.Domain.Enums;
using Teczter.Services.Dtos.RequestDtos.TestRequests;
using Teczter.Services.ServiceInterfaces;
using Teczter.WebApi.DTOs;

namespace Teczter.Services.Controllers;

[Route("api/[controller]")]
[ApiController]
public class TestController : ControllerBase
{
    private readonly ITestAdministrationService _testAdministrationService;

    public TestController(ITestAdministrationService testAdministrationService)
    {
        _testAdministrationService = testAdministrationService;
    }

    [HttpGet]
    public async Task<IActionResult> GetTestSearchResults(
        [FromQuery] string? testRoundName,
        [FromQuery] Pillar? pillar,
        [FromQuery] string? assignedUserUsername,
        [FromQuery] string? testTitle,
        [FromQuery] ExecutionStateType? testState
        )
    {
        var request = new TestSearchRequest(testRoundName, pillar, assignedUserUsername, testTitle, testState);
        var tests = await _testAdministrationService.GetTestSearchResults(request);
        var dtos = tests.Select(x => new TestDto(x)).ToList();

        return Ok(dtos);
    }
}