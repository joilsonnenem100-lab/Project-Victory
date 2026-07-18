using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProjectVictory.Application.Game;

namespace ProjectVictory.API.Controllers;

[Route("api/game")]
public sealed class GameController : BaseApiController
{
    private readonly EnterGameHandler _handler;


    public GameController(
        EnterGameHandler handler)
    {
        _handler = handler;
    }


    [Authorize]
    [HttpPost("enter")]
    public async Task<IActionResult> Enter()
    {
        var result =
            await _handler.Handle(
                new EnterGameCommand());


        return Success(result);
    }
}