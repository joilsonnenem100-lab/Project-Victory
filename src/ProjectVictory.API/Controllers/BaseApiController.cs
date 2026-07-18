using Microsoft.AspNetCore.Mvc;

namespace ProjectVictory.API.Controllers;

[ApiController]
public abstract class BaseApiController : ControllerBase
{
    protected IActionResult Success<T>(T result)
    {
        return Ok(result);
    }
}