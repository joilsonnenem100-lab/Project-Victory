using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProjectVictory.API.Authentication;
using ProjectVictory.Application.Admin;

namespace ProjectVictory.API.Controllers;

// PV-ADMIN-010 (DEC-20261106-12): ferramenta de administração — XP rate do servidor.
// Só conta administradora (policy "Administrator"): sem token → 401; jogador comum → 403.
// Controller fino: os casos de uso (e a nova checagem de papel) ficam na Application.
[Authorize(Policy = AdministratorAuthorizationHandler.PolicyName)]
[Route("api/admin")]
public sealed class AdminController : BaseApiController
{
    [HttpGet("xp-rate")]
    public async Task<IActionResult> GetExperienceRate(
        [FromServices] GetExperienceRateHandler handler,
        CancellationToken cancellationToken)
    {
        return Success(await handler.Handle(cancellationToken));
    }

    // Valor inválido (<= 0 ou acima do limite) → 400. Vale para as NOVAS recompensas.
    [HttpPut("xp-rate")]
    public async Task<IActionResult> SetExperienceRate(
        [FromBody] SetExperienceRateRequest request,
        [FromServices] SetExperienceRateHandler handler,
        CancellationToken cancellationToken)
    {
        return Success(await handler.Handle(new SetExperienceRateCommand(request.Value), cancellationToken));
    }
}

public sealed record SetExperienceRateRequest(decimal Value);
