using Microsoft.AspNetCore.Mvc;
using Teczter.Services.ServiceInterfaces;
using Teczter.WebApi.DTOs;

namespace Teczter.Services.Controllers;

[Route("api/[controller]")]
[ApiController]
public class TestController : ControllerBase
{
    private readonly ITestService _testService;

    public TestController(ITestService testService)
    {
        _testService = testService;
    }

    [HttpGet]
    public async Task<IActionResult> GetTestSearchResults([FromQuery] string? testname, string? pillarOwner)
    {
        var tests = await _testService.GetTestSearchResults(testname, pillarOwner);

        return Ok(tests.Select(x => new TestDetailedDto(x)).ToList());
    }
}