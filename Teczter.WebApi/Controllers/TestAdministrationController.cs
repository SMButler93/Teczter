using Microsoft.AspNetCore.Mvc;
using System.Linq.Expressions;
using Teczter.Domain.Entities;
using Teczter.Services.ServiceInterfaces;
using Teczter.WebApi.DTOs;

namespace Teczter.Services.Controllers;

[Route("api/[controller]")]
[ApiController]
public class TestAdministrationController : ControllerBase
{
    private readonly ITestAdministrationService _testAdministrationService;

    public TestAdministrationController(ITestAdministrationService testAdministrationService)
    {
        _testAdministrationService = testAdministrationService;
    }

    [HttpGet]
    [Route("/{testRoundName}/Tests")]
    public async Task<IActionResult> GetTestsInTestRound(
        [FromRoute] string testRoundName, 
        [FromQuery] string? filter = null, 
        [FromQuery] string? filterValue = null,
        [FromQuery] string orderBy = "asc")
    {
        var tests = await _testAdministrationService.GetTestsInTestRound(testRoundName, filter, filterValue, orderBy);

        var dtos = tests.Select(x => new TestDto(x)).ToList();

        return Ok(dtos);
    }

    [HttpGet]
    [Route("/Test/{id:guid}")]
    public async Task<IActionResult> GetTestById([FromRoute] Guid id)
    {
        var test = await _testAdministrationService.GetTestById(id);

        if (test == default)
        {
            return NotFound($"No test found with ID: {id}");
        }

        return Ok(new TestDto(test));
    }

    [HttpGet]
    [Route("/AssignedTests/{userId:guid}")]
    public async Task<IActionResult> GetAllUserAssignedTests(UserEntity user)
    {
        var tests = await _testAdministrationService.GetAllUsersAssignedTests(user.Id);

        var dtos = tests.Select(x => new TestDto(x)).ToList();

        return Ok(dtos);
    }
}