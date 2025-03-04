using Microsoft.AspNetCore.Mvc;
using Teczter.Domain.Exceptions;
using Teczter.Services.DTOs.Request;
using Teczter.Services.RequestDtos.Request;
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
    public async Task<IActionResult> GetTestSearchResults([FromQuery] string? testname, [FromQuery] string? owningDepartment)
    {
        var tests = await _testService.GetTestSearchResults(testname, owningDepartment);

        var testDtos = tests.Select(x => new TestBasicDto(x)).ToList();

        return Ok(testDtos.OrderBy(x => x.Department).ThenBy(x => x.Title));
    }

    [HttpGet]
    [Route("/TestDetails/{id:int}")]
    public async Task<IActionResult> GetTest(int id)
    {
        var test = await _testService.GetTestById(id);

        if (test == null)
        {
            return NotFound($"Test {id} does not exist");
        }

        return Ok(new TestDetailedDto(test));
    }

    [HttpDelete]
    [Route("/TestDetails/{id:int}/Delete")]
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
    [Route("/CreateTest")]
    public async Task<IActionResult> CreateTest([FromBody] CreateTestRequestDto request)
    {
        try
        {
            var validatedtest = await _testService.CreateNewTest(request);
     
            if (!validatedtest.IsValid)
            {
                return BadRequest(validatedtest.ErrorMessages);
            }

            var dto = new TestDetailedDto(validatedtest.Value!);

            return CreatedAtAction(nameof(GetTest), new { dto.Id }, dto);
        }
        catch(TeczterValidationException ex)
        {
            return BadRequest(ex);
        }
    }

    [HttpPut]
    [Route("/UpdateTestDetails")]
    public async Task<IActionResult> UpdateTest([FromQuery] int id, [FromBody] UpdateTestRequestDto request)
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

        return Ok(new TestDetailedDto(test));
    }

    [HttpPut]
    [Route("/{id:int}/AddUrlResource")]
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

        return Ok(new TestDetailedDto(test));
    }

    [HttpPut]
    [Route("/{id:int}/RemoveUrlResource")]
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

            return Ok(new TestDetailedDto(test));
        }
        catch(TeczterValidationException ex)
        {
            return BadRequest(ex);
        }
    }

    [HttpPut]
    [Route("/{id:int}/AddTestStep")]
    public async Task<IActionResult> AddTestStep(int id, [FromBody] TestStepCommandRequestDto request)
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

        return Ok(new TestDetailedDto(test));
    }

    [HttpPut]
    [Route("/{testId:int}/RemoveTestStep/{testStepId:int}")]
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

            return Ok(new TestDetailedDto(test));
        }
        catch (TeczterValidationException ex)
        {
            return BadRequest(ex);
        }
    }
}