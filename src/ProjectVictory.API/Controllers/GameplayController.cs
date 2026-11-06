using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProjectVictory.Application.Gameplay;

namespace ProjectVictory.API.Controllers;

[Authorize] // PV-SEC-001
[Route("api/gameplay")]
public sealed class GameplayController : BaseApiController
{
    private readonly GameplayActionExecutionHandler _handler;

    public GameplayController(
        GameplayActionExecutionHandler handler)
    {
        _handler = handler;
    }

    [HttpPost("action")]
    public async Task<IActionResult> Execute(
        [FromBody] GameplayActionExecutionCommand command)
    {
        var result = await _handler.Handle(command);

        return Success(result);
    }
}
