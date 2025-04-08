using Microsoft.AspNetCore.Mvc;
using Teczter.Domain.Exceptions;
using Teczter.Services.RequestDtos;
using Teczter.Services.ServiceInterfaces;
using Teczter.WebApi.ResponseDtos;

namespace Teczter.Services.Controllers;

[Route("Teczter/[controller]")]
[ApiController]
public class TestController : ControllerBase
{
    private readonly ITestService _testService;

    public TestController(ITestService testService)
    {
        _testService = testService;
    }

    [HttpGet]
    public async Task<IActionResult> GetTestSearchResults([FromQuery] int pageNumber, [FromQuery] string? testName, [FromQuery] string? owningDepartment)
    {
        var tests = await _testService.GetTestSearchResults(pageNumber, testName, owningDepartment);

        var testDtos = tests.Select(x => new TestDto(x)).ToList();

        return Ok(testDtos);
    }

    [HttpGet]
    [Route("{id:int}")]
    public async Task<IActionResult> GetTest(int id)
    {
        var test = await _testService.GetTestById(id);

        if (test == null)
        {
            return NotFound($"Test {id} does not exist");
        }

        return Ok(new TestDto(test));
    }

    [HttpDelete]
    [Route("{id:int}/Delete")]
    public async Task<IActionResult> DeleteTest(int id)
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
    [Route("CreateTest")]
    public async Task<IActionResult> CreateTest([FromBody] CreateTestRequestDto request)
    {
        try
        {
            var validatedtest = await _testService.CreateNewTest(request);
     
            if (!validatedtest.IsValid)
            {
                return BadRequest(validatedtest.ErrorMessages);
            }

            var dto = new TestDto(validatedtest.Value!);

            return CreatedAtAction(nameof(GetTest), new { dto.Id }, dto);
        }
        catch(TeczterValidationException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPatch]
    [Route("{id:int}/UpdateTestDetails")]
    public async Task<IActionResult> UpdateTest(int id, [FromBody] UpdateTestRequestDto request)
    {
        var test = await _testService.GetTestById(id);

        if (test == null)
        {
            return NotFound($"Test {id} does not exist");
        }

        var validatedTest = await _testService.UpdateTest(test, request);

        if (!validatedTest.IsValid)
        {
            return BadRequest(validatedTest.ErrorMessages);
        }

        return Ok(new TestDto(test));
    }

    [HttpPut]
    [Route("{id:int}/AddUrlResource")]
    public async Task<IActionResult> AddLinkUrl(int id, [FromBody] string url)
    {
        var test = await _testService.GetTestById(id);

        if (test == null)
        {
            return NotFound($"Test {id} does not exist");
        }

        var validatedTest = await _testService.AddLinkUrl(test, url);

        if (!validatedTest.IsValid)
        {
            return BadRequest(validatedTest.ErrorMessages);
        }

        return Ok(new TestDto(test));
    }

    [HttpPut]
    [Route("{id:int}/RemoveUrlResource")]
    public async Task<IActionResult> RemoveLinkUrl(int id, [FromBody] string url)
    {
        var test = await _testService.GetTestById(id);

        if (test == null)
        {
            return NotFound($"test {id} does not exist.");
        }

        try
        {
            var validatedTest = await _testService.RemoveLinkUrl(test, url);

            if (!validatedTest.IsValid)
            {
                return BadRequest(validatedTest.ErrorMessages);
            }

            return Ok(new TestDto(test));
        }
        catch(TeczterValidationException ex)
        {
            return BadRequest(ex);
        }
    }

    [HttpPut]
    [Route("{id:int}/AddTestStep")]
    public async Task<IActionResult> AddTestStep(int id, [FromBody] CreateTestStepRequestDto request)
    {
        var test = await _testService.GetTestById(id);

        if (test == null)
        {
            return NotFound($"Test {id} does not exist");
        }

        var validatedTest = await _testService.AddTestStep(test, request);

        if (!validatedTest.IsValid)
        {
            return BadRequest(validatedTest.ErrorMessages);
        }

        return Ok(new TestDto(test));
    }

    [HttpPut]
    [Route("{testId:int}/RemoveTestStep/{testStepId:int}")]
    public async Task<IActionResult> RemoveTestStep(int testId, int testStepId)
    {
        var test = await _testService.GetTestById(testId);

        if (test == null)
        {
            return NotFound($"test {testId} does not exist.");
        }

        try
        {
            var validatedTest = await _testService.RemoveTestStep(test, testStepId);

            if (!validatedTest.IsValid)
            {
                return BadRequest(validatedTest.ErrorMessages);
            }

            return Ok(new TestDto(test));
        }
        catch (TeczterValidationException ex)
        {
            return BadRequest(ex);
        }
    }

    [HttpPut]
    [Route("{testId:int}/UpdateTestStep/{testStepId:int}")]
    public async Task<IActionResult> UpdateTestStep(int testId, int testStepId, [FromBody] UpdateTestStepRequestDto request)
    {
        var test = await _testService.GetTestById(testId);

        if (test == null)
        {
            return NotFound($"test {testId} does not exist.");
        }

        try
        {
            var validatedTest = await _testService.UpdateTestStep(test, testStepId, request);

            if (!validatedTest.IsValid)
            {
                return BadRequest(validatedTest.ErrorMessages);
            }

            return Ok(new TestDto(test));
        }
        catch (TeczterValidationException ex)
        {
            return BadRequest(ex);
        }
    }
}