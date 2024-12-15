using Microsoft.AspNetCore.Mvc;
using Teczter.Services.ServiceInterfaces;

namespace Teczter.Services.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestRoundController : ControllerBase
    {
        private readonly ITestRoundService _testRoundService;

        public TestRoundController(ITestRoundService testRoundService)
        {
            _testRoundService = testRoundService;
        }
    }
}
