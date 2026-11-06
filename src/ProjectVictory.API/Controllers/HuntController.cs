using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using ProjectVictory.Application.Hunt;


namespace ProjectVictory.API.Controllers;


[Authorize] // PV-SEC-001
[Route("api/hunt")]
public sealed class HuntController : BaseApiController
{
    private readonly ExecuteHuntHandler _handler;

    private readonly AttackEncounterHandler _attackHandler;



    public HuntController(
        ExecuteHuntHandler handler,
        AttackEncounterHandler attackHandler)
    {
        _handler = handler;

        _attackHandler = attackHandler;
    }



    [HttpPost("execute")]
    public async Task<IActionResult> Execute(
        [FromBody] ExecuteHuntCommand command)
    {
        var result =
            await _handler.Handle(
                command);


        return Success(
            result);
    }



    [HttpPost("attack")]
    public async Task<IActionResult> Attack(
        [FromBody] AttackEncounterCommand command)
    {
        // PV-SEC-001 (D3): antes o Task era serializado sem await.
        var result =
            await _attackHandler.Handle(
                command);


        return Success(
            result);
    }
}