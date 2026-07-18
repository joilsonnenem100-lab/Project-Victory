using Microsoft.AspNetCore.Mvc;
using ProjectVictory.Application.Game;

namespace ProjectVictory.API.Controllers;

[Route("api/game")]
public sealed class GameController : BaseApiController
{
    private readonly EnterGameHandler _handler;

    public GameController(EnterGameHandler handler)
    {
        _handler = handler;
    }

    [HttpPost("enter")]
    public async Task<IActionResult> Enter(EnterGameCommand command)
    {
        var result = await _handler.Handle(command);

        return Success(result);
    }
}