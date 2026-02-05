using Microsoft.AspNetCore.Mvc;
using Teczter.WebApi.RequestValidations.ValidationAttributes;

namespace Teczter.WebApi.Controllers
{
    [Route("Teczter/[controller]")]
    [ApiController]
    [ValidateRequest]
    public abstract class TeczterControllerBase : ControllerBase
    {
    }
}
