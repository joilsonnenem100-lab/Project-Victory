using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProjectVictory.Application.HuntAnalyzer;

namespace ProjectVictory.API.Controllers;

// PV-HUD-ANALYZER-010 (DEC-20261106-01/08, PO-47): Hunt Analyzer calculado no servidor — só leitura.
// Posse pelo token (PV-SEC-001): personagem de outra conta ou inexistente → 403.
[Authorize]
[Route("api/hunt/analyzer")]
public sealed class HuntAnalyzerController : BaseApiController
{
    // Tempo real durante a Hunt; sem Hunt em andamento, o resumo da última Hunt encerrada.
    [HttpGet("{characterId:guid}")]
    public async Task<IActionResult> Get(
        Guid characterId,
        [FromServices] GetHuntAnalyzerHandler handler,
        CancellationToken cancellationToken)
    {
        return Success(
            await handler.Handle(new GetHuntAnalyzerQuery(characterId), cancellationToken));
    }

    // Histórico das últimas Hunts (mais recente primeiro).
    [HttpGet("{characterId:guid}/history")]
    public async Task<IActionResult> History(
        Guid characterId,
        [FromServices] GetHuntAnalyzerHistoryHandler handler,
        CancellationToken cancellationToken)
    {
        return Success(
            await handler.Handle(new GetHuntAnalyzerHistoryQuery(characterId), cancellationToken));
    }
}
