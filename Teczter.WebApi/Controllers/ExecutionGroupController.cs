using Microsoft.AspNetCore.Mvc;
using Teczter.Services.ServiceInterfaces;

namespace Teczter.Services.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExecutionGroupController : ControllerBase
    {
        private readonly ITestRoundService _testRoundService;

        public ExecutionGroupController(ITestRoundService testRoundService)
        {
            _testRoundService = testRoundService;
        }
    }
}
