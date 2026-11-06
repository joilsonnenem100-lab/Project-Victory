using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProjectVictory.Application.Game;
using ProjectVictory.Application.GameSession;

namespace ProjectVictory.API.Controllers;

[Authorize] // PV-SEC-001: na classe, para nenhuma ação nova nascer pública
[Route("api/game")]
public sealed class GameController : BaseApiController
{
    private readonly EnterGameHandler _handler;
    private readonly HeartbeatHandler _heartbeat;


    public GameController(
        EnterGameHandler handler,
        HeartbeatHandler heartbeat)
    {
        _handler = handler;
        _heartbeat = heartbeat;
    }

    // PV-RUNTIME-010 (ADR-057 v1.2): heartbeat HTTP do cliente (sem WebSocket). Identidade pelo token.
    [HttpPost("heartbeat")]
    public async Task<IActionResult> Heartbeat()
    {
        var result =
            await _heartbeat.Handle(
                new HeartbeatCommand());
        return Success(result);
    }


    [HttpPost("enter")]
    public async Task<IActionResult> Enter()
    {
        var result =
            await _handler.Handle(
                new EnterGameCommand());


        return Success(result);
    }
}