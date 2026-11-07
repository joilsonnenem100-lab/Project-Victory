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



    // PV-HUNT-014 (PO-09/PO-10/PO-55/PO-56): catálogo somente leitura de regiões e Hunts (nome, tier/
    // título/Level recomendados — só informativos — e monstros). Exige token; sem dados do jogador.
    [HttpGet("catalog")]
    public IActionResult Catalog(
        [FromServices] GetHuntCatalogHandler catalog)
    {
        return Success(
            catalog.Handle());
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

    // PV-HUNT-011 (DEC-20261106-22): saída manual da Hunt (pipeline único; volta à "Victoria").
    [HttpPost("leave")]
    public async Task<IActionResult> Leave(
        [FromBody] LeaveHuntCommand command,
        [FromServices] LeaveHuntHandler leaveHandler)
    {
        var result =
            await leaveHandler.Handle(
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
