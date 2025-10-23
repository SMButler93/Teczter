using Microsoft.AspNetCore.Mvc;
using Teczter.Domain.Exceptions;
using Teczter.Services.RequestDtos.Tests;
using Teczter.Services.RequestDtos.TestSteps;
using Teczter.Services.ServiceInterfaces;
using Teczter.WebApi.ResponseDtos;

namespace Teczter.WebApi.Controllers;

[Route("Teczter/[controller]")]
[ApiController]
public class TestsController(ITestService testService) : ControllerBase
{
    private readonly ITestService _testService = testService;

    [HttpGet]
    public async Task<ActionResult<List<TestDto>>> GetTestSearchResults([FromQuery] string? testName, [FromQuery] string? owningDepartment, [FromQuery] int pageNumber = 1)
    {
        var tests = await _testService.GetTestSearchResults(pageNumber, testName, owningDepartment);

        var testDtos = tests.Select(x => new TestDto(x)).ToList();

        return Ok(testDtos);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<TestDto>> GetTest(int id)
    {
        var test = await _testService.GetTestById(id);

        if (test is null)
        {
            return NotFound($"Test {id} does not exist");
        }

        return Ok(new TestDto(test));
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteTest(int id)
    {
        var test = await _testService.GetTestById(id);

        if (test is null)
        {
            return NotFound($"Test {id} does not exist");
        }

        await _testService.DeleteTest(test);

        return NoContent();
    }

    [HttpPost]
    public async Task<ActionResult<TestDto>> CreateTest([FromBody] CreateTestRequestDto request)
    {
        var result = await _testService.CreateNewTest(request);

        if (!result.IsValid)
        {
            return BadRequest(result.ErrorMessages);
        }

        return Ok(new TestDto(result.Value!));
    }

    [HttpPatch("{id:int}")]
    public async Task<ActionResult<TestDto>> UpdateTest(int id, [FromBody] UpdateTestRequestDto request)
    {
        var test = await _testService.GetTestById(id);

        if (test is null)
        {
            return NotFound($"Test {id} does not exist");
        }

        var result = await _testService.UpdateTest(test, request);

        if (!result.IsValid)
        {
            return BadRequest(result.ErrorMessages);
        }

        return Ok(new TestDto(test));
    }

    [HttpPost("{id:int}/links")]
    public async Task<ActionResult<TestDto>> AddLinkUrl(int id, [FromBody] string url)
    {
        var test = await _testService.GetTestById(id);

        if (test is null)
        {
            return NotFound($"Test {id} does not exist");
        }

        var result = await _testService.AddLinkUrl(test, url);

        if (!result.IsValid)
        {
            return BadRequest(result.ErrorMessages);
        }

        return Ok(new TestDto(test));
    }

    [HttpDelete("{id:int}/links")]
    public async Task<ActionResult<TestDto>> RemoveLinkUrl(int id, [FromBody] string url)
    {
        var test = await _testService.GetTestById(id);

        if (test is null)
        {
            return NotFound($"test {id} does not exist.");
        }

        var result = await _testService.RemoveLinkUrl(test, url);

        if (!result.IsValid)
        {
            return BadRequest(result.ErrorMessages);
        }

        return Ok(new TestDto(test));
    }

    [HttpPost("{id:int}/Steps")]
    public async Task<ActionResult<TestDto>> AddTestStep(int id, [FromBody] CreateTestStepRequestDto request)
    {
        var test = await _testService.GetTestById(id);

        if (test is null)
        {
            return NotFound($"Test {id} does not exist");
        }

        var result = await _testService.AddTestStep(test, request);

        if (!result.IsValid)
        {
            return BadRequest(result.ErrorMessages);
        }

        return Ok(new TestDto(test));
    }

    [HttpDelete("{testId:int}/Steps/{testStepId:int}")]
    public async Task<ActionResult<TestDto>> RemoveTestStep(int testId, int testStepId)
    {
        var test = await _testService.GetTestById(testId);

        if (test is null)
        {
            return NotFound($"test {testId} does not exist.");
        }

        var result = await _testService.RemoveTestStep(test, testStepId);

        if (!result.IsValid)
        {
            return BadRequest(result.ErrorMessages);
        }

        return Ok(new TestDto(test));
    }

    [HttpPatch("{testId:int}/Steps/{testStepId:int}")]
    public async Task<ActionResult<TestDto>> UpdateTestStep(int testId, int testStepId, [FromBody] UpdateTestStepRequestDto request)
    {
        var test = await _testService.GetTestById(testId);

        if (test is null)
        {
            return NotFound($"test {testId} does not exist.");
        }

        var result = await _testService.UpdateTestStep(test, testStepId, request);

        if (!result.IsValid)
        {
            return BadRequest(result.ErrorMessages);
        }

        return Ok(new TestDto(test));
    }
}