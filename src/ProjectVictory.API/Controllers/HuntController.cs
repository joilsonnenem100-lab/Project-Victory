using Microsoft.AspNetCore.Mvc;
using ProjectVictory.Application.Hunt;

namespace ProjectVictory.API.Controllers;

[Route("api/hunt")]
public sealed class HuntController : BaseApiController
{
    private readonly ExecuteHuntHandler _handler;

    public HuntController(ExecuteHuntHandler handler)
    {
        _handler = handler;
    }

    [HttpPost("execute")]
    public async Task<IActionResult> Execute(ExecuteHuntCommand command)
    {
        var result = await _handler.Handle(command);

        return Success(result);
    }
}