﻿using Microsoft.AspNetCore.Mvc;
using Teczter.Services.ServiceInterfaces;
using Teczter.WebApi.RequestDTOs;
using Teczter.WebApi.ResponseDTOs;

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
            return NotFound($"Test {id} could not be found.");
        }

        return Ok(new TestDetailedDto(test));
    }

    [HttpDelete]
    [Route("/TestDetails/{id:guid}/Delete")]
    public async Task<IActionResult> DeleteTest(Guid id)
    {
        await _testService.DeleteTest(id);

        return NoContent();
    }

    [HttpPost]
    public async Task<IActionResult> CreateTest([FromBody] TestCreationDto request)
    {
        var entity = request.MapToEntity();

        await _testService.CreateNewTest(entity);

        var dto = new TestDetailedDto(entity);

        return CreatedAtAction(nameof(GetTest), new { entity.Id }, dto);
    }
}