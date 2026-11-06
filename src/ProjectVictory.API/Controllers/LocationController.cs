using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProjectVictory.Application.World;

namespace ProjectVictory.API.Controllers;

[Authorize] // PV-SEC-001
[Route("api/location")]
public sealed class LocationController : BaseApiController
{
    private readonly ResolveLocationActionsHandler _handler;

    public LocationController(
        ResolveLocationActionsHandler handler)
    {
        _handler = handler;
    }

    [HttpPost("actions")]
    public IActionResult Actions(
        [FromBody] ResolveLocationActionsCommand command)
    {
        var result =
            _handler.Handle(command);

        return Success(result);
    }
}
