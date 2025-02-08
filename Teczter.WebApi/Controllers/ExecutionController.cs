using Microsoft.AspNetCore.Mvc;
using Teczter.Services.ServiceInterfaces;

namespace Teczter.Services.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExecutionController : ControllerBase
    {
        private readonly ITestService _testService;

        public ExecutionController(ITestService testService)
        {
            _testService = testService;
        }
    }
}
