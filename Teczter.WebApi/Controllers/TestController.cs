using Microsoft.AspNetCore.Mvc;
using Teczter.Services.DTOs.Request;
using Teczter.Services.ServiceInterfaces;
using Teczter.WebApi.ResponseDtos;

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
    public async Task<IActionResult> GetTestSearchResults([FromQuery] string? testname, [FromQuery] string? pillarOwner)
    {
        var tests = await _testService.GetTestSearchResults(testname, pillarOwner);

        var testDtos = tests.Select(x => new TestBasicDto(x)).ToList();

        return Ok(testDtos.OrderBy(x => x.Pillar).ThenBy(x => x.Title));
    }

    [HttpGet]
    [Route("/TestDetails/{id:guid}")]
    public async Task<IActionResult> GetTest(Guid id)
    {
        var test = await _testService.GetTestById(id);

        if (test == null)
        {
            return NotFound($"Test {id} does not exist");
        }

        return Ok(new TestDetailedDto(test));
    }

    [HttpDelete]
    [Route("/TestDetails/{id:guid}/Delete")]
    public async Task<IActionResult> DeleteTest(Guid id)
    {
        var test = await _testService.GetTestById(id);

        if (test == null)
        {
            return NotFound($"Test {id} does not exist");
        }

        await _testService.DeleteTest(test);

        return NoContent();
    }

    [HttpPost]
    [Route("/CreateTest")]
    public async Task<IActionResult> CreateTest([FromBody] TestCommandRequestDto request)
    {
        var test = await _testService.CreateNewTest(request);

        var dto = new TestDetailedDto(test);

        return CreatedAtAction(nameof(GetTest), new { dto.Id }, dto);
    }

    [HttpPut]
    [Route("/{id:guid}/Update")]
    public async Task<IActionResult> UpdateTest(Guid id, TestCommandRequestDto request)
    {
        var test = await _testService.GetTestById(id);

        if (test == null)
        {
            return NotFound($"Test {id} does not exist");
        }

        await _testService.UpdateTest(test, request);

        return Ok(new TestDetailedDto(test));
    }

    [HttpPut]
    [Route("/{id:guid}/AddUrlResource")]
    public async Task<IActionResult> AddLinkUrl(Guid id, [FromBody] string url)
    {
        var test = await _testService.GetTestById(id);

        if (test == null)
        {
            return NotFound($"Test {id} does not exist");
        }

        await _testService.AddLinkUrl(test, url);

        return Ok(new TestDetailedDto(test));
    }

    [HttpPut]
    [Route("/{id:guid}/RemoveUrlResource")]
    public async Task<IActionResult> RemoveLinkUrl(Guid id, [FromBody] string url)
    {
        var test = await _testService.GetTestById(id);

        if (test == null)
        {
            return NotFound($"test {id} does not exist.");
        }

        await _testService.RemoveLinkUrl(test, url);

        return Ok(new TestDetailedDto(test));
    }

    [HttpPut]
    [Route("/{id:guid}/AddTestStep")]
    public async Task<IActionResult> AddTestStep(Guid id, [FromBody] TestStepCommandRequestDto request)
    {
        var test = await _testService.GetTestById(id);

        if (test == null)
        {
            return NotFound($"Test {id} does not exist");
        }

        await _testService.AddTestStep(test, request);

        return Ok(new TestDetailedDto(test));
    }

    [HttpPut]
    [Route("/{testId:guid}/RemoveTestStep/{testStepId:guid}")]
    public async Task<IActionResult> RemoveTestStep(Guid testId, Guid testStepId)
    {
        var test = await _testService.GetTestById(testId);

        if (test == null)
        {
            return NotFound($"test {testId} does not exist.");
        }

        await _testService.RemoveTestStep(test, testStepId);

        return Ok(new TestDetailedDto(test));
    }
}