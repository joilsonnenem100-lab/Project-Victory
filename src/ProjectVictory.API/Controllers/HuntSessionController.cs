using Microsoft.AspNetCore.Mvc;
using ProjectVictory.Application.Hunt;

namespace ProjectVictory.API.Controllers;

[Route("api/hunt-session")]
public sealed class HuntSessionController : BaseApiController
{
    private readonly StartHuntSessionHandler _startHandler;
    private readonly StopHuntSessionHandler _stopHandler;


    public HuntSessionController(
        StartHuntSessionHandler startHandler,
        StopHuntSessionHandler stopHandler)
    {
        _startHandler = startHandler;
        _stopHandler = stopHandler;
    }


    [HttpPost("start")]
    public async Task<IActionResult> Start(
        [FromBody] StartHuntSessionCommand command)
    {
        var result =
            await _startHandler.Handle(command);

        return Success(result);
    }


    [HttpPost("stop")]
    public async Task<IActionResult> Stop(
        [FromBody] StopHuntSessionCommand command)
    {
        await _stopHandler.Handle(command);

        return Success(new { });
    }
}
