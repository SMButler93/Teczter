using Microsoft.AspNetCore.Mvc;
using Teczter.Services.ComposersAndBuilders;
using Teczter.Services.RequestDtos.Tests;
using Teczter.Services.RequestDtos.TestSteps;
using Teczter.Services.ServiceInterfaces;
using Teczter.WebApi.ResponseDtos;

namespace Teczter.WebApi.Controllers;

[Route("Teczter/[controller]")]
[ApiController]
public class TestsController(ITestService testService) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<List<TestDto>>> GetTestSearchResults(
        [FromQuery] string? testName, 
        [FromQuery] string? owningDepartment, 
        CancellationToken ct, 
        [FromQuery] int pageNumber = 1)
    {
        var tests = await testService.GetTestSearchResults(pageNumber, testName, owningDepartment, ct);

        var testDtos = tests.Select(x => new TestDto(x)).ToList();

        return Ok(testDtos);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<TestDto>> GetTest(int id, CancellationToken ct)
    {
        var test = await testService.GetTestById(id, ct);

        if (test is null)
        {
            return NotFound($"Test {id} does not exist");
        }

        return Ok(new TestDto(test));
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteTest(int id, CancellationToken ct)
    {
        var test = await testService.GetTestById(id, ct);

        if (test is null)
        {
            return NotFound($"Test {id} does not exist");
        }

        await testService.DeleteTest(test, ct);

        return NoContent();
    }

    [HttpPost]
    public async Task<ActionResult<TestDto>> CreateTest([FromBody] CreateTestRequestDto request, CancellationToken ct)
    {
        var result = await testService.CreateNewTest(request, ct);

        if (result.IsValid)
        {
            return Ok(new TestDto(result.Value!));
        }
        
        var message = ErrorMessageResponseBuilder.BuildErrorMessage(result.ErrorMessages!);

        return BadRequest(message);

    }

    [HttpPatch("{id:int}")]
    public async Task<ActionResult<TestDto>> UpdateTest(int id, [FromBody] UpdateTestRequestDto request, CancellationToken ct)
    {
        var test = await testService.GetTestById(id, ct);

        if (test is null)
        {
            return NotFound($"Test {id} does not exist");
        }

        var result = await testService.UpdateTest(test, request, ct);

        if (result.IsValid)
        {
            return Ok(new TestDto(test));
        }
        
        var message = ErrorMessageResponseBuilder.BuildErrorMessage(result.ErrorMessages!);

        return BadRequest(message);

    }

    [HttpPost("{id:int}/links")]
    public async Task<ActionResult<TestDto>> AddLinkUrl(int id, [FromBody] string url, CancellationToken ct)
    {
        var test = await testService.GetTestById(id, ct);

        if (test is null)
        {
            return NotFound($"Test {id} does not exist");
        }

        var result = await testService.AddLinkUrl(test, url, ct);

        if (result.IsValid)
        {
            return Ok(new TestDto(test));
        }
        
        var message = ErrorMessageResponseBuilder.BuildErrorMessage(result.ErrorMessages!);

        return BadRequest(message);

    }

    [HttpDelete("{id:int}/links")]
    public async Task<ActionResult<TestDto>> RemoveLinkUrl(int id, [FromBody] string url, CancellationToken ct)
    {
        var test = await testService.GetTestById(id, ct);

        if (test is null)
        {
            return NotFound($"Test {id} does not exist.");
        }

        var result = await testService.RemoveLinkUrl(test, url, ct);

        if (result.IsValid)
        {
            return Ok(new TestDto(test));
        }
        
        var message = ErrorMessageResponseBuilder.BuildErrorMessage(result.ErrorMessages!);

        return BadRequest(message);
    }

    [HttpPost("{id:int}/Steps")]
    public async Task<ActionResult<TestDto>> AddTestStep(int id, [FromBody] CreateTestStepRequestDto request, CancellationToken ct)
    {
        var test = await testService.GetTestById(id, ct);

        if (test is null)
        {
            return NotFound($"Test {id} does not exist");
        }

        var result = await testService.AddTestStep(test, request, ct);

        if (result.IsValid)
        {
            return Ok(new TestDto(test));
        }
        
        var message = ErrorMessageResponseBuilder.BuildErrorMessage(result.ErrorMessages!);

        return BadRequest(message);

    }

    [HttpDelete("{testId:int}/Steps/{testStepId:int}")]
    public async Task<ActionResult<TestDto>> RemoveTestStep(int testId, int testStepId, CancellationToken ct)
    {
        var test = await testService.GetTestById(testId, ct);

        if (test is null)
        {
            return NotFound($"Test {testId} does not exist.");
        }

        var result = await testService.RemoveTestStep(test, testStepId, ct);

        if (result.IsValid)
        {
            return Ok(new TestDto(test));
        }
        
        var message = ErrorMessageResponseBuilder.BuildErrorMessage(result.ErrorMessages!);

        return BadRequest(message);

    }

    [HttpPatch("{testId:int}/Steps/{testStepId:int}")]
    public async Task<ActionResult<TestDto>> UpdateTestStep(int testId, int testStepId, [FromBody] UpdateTestStepRequestDto request, CancellationToken ct)
    {
        var test = await testService.GetTestById(testId, ct);

        if (test is null)
        {
            return NotFound($"Test {testId} does not exist.");
        }

        var result = await testService.UpdateTestStep(test, testStepId, request, ct);

        if (result.IsValid)
        {
            return Ok(new TestDto(test));
        }
        
        var message = ErrorMessageResponseBuilder.BuildErrorMessage(result.ErrorMessages!);

        return BadRequest(message);
    }
}