using Microsoft.AspNetCore.Mvc;

using ProjectVictory.Application.Hunt;


namespace ProjectVictory.API.Controllers;


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
    public IActionResult Attack(
        [FromBody] AttackEncounterCommand command)
    {
        var result =
            _attackHandler.Handle(
                command);


        return Success(
            result);
    }
}