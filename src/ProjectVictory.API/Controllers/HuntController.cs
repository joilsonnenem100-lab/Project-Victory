using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using ProjectVictory.Application.Hunt;
using ProjectVictory.Application.Supply;


namespace ProjectVictory.API.Controllers;


[Authorize] // PV-SEC-001
[Route("api/hunt")]
public sealed class HuntController : BaseApiController
{
    private readonly ExecuteHuntHandler _handler;

    private readonly AttackEncounterHandler _attackHandler;
    private readonly SetSupplyPolicyHandler _supplyHandler;



    public HuntController(
        ExecuteHuntHandler handler,
        AttackEncounterHandler attackHandler,
        SetSupplyPolicyHandler supplyHandler)
    {
        _supplyHandler = supplyHandler;
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

    // PV-SUPPLY-010: configura o uso de poções (vale a partir do próximo turno). 400 se inválido;
    // 403 se o personagem não for da conta do token.
    [HttpPost("supplies")]
    public async Task<IActionResult> Supplies(
        [FromBody] SetSupplyPolicyCommand command)
    {
        var result =
            await _supplyHandler.Handle(
                command);

        return Success(
            result);
    }
}
