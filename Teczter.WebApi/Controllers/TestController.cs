using Microsoft.AspNetCore.Mvc;
using Teczter.Services.ServiceInterfaces;
using Teczter.WebApi.DTOs.Request;
using Teczter.WebApi.DTOs.Response;
using Teczter.WebApi.Mappers;

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

        if (test == default)
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
    public async Task<IActionResult> CreateTest([FromBody] CreateTestRequestDto request)
    {
        var entity = DtoMapper.MapToEntity(request);

        entity = await _testService.CreateNewTest(entity);

        var dto = new TestDetailedDto(entity);

        return CreatedAtAction(nameof(GetTest), new { dto.Id }, dto);
    }

    [HttpPut]
    [Route("/{id:guid}/Update")]
    public async Task<IActionResult> UpdateTest(Guid id, CreateTestRequestDto request) 
    {
        var currentTest = await _testService.GetTestById(id);

        if (currentTest == null)
        {
            return NotFound($"Test {id} does not exist");
        }

        var entity = DtoMapper.MapToEntity(request);

        entity = await _testService.UpdateTest(currentTest, entity);

        return Ok(new TestDetailedDto(entity));
    }
}