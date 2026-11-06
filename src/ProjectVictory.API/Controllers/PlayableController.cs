using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ProjectVictory.API.Controllers;

[Authorize] // PV-SEC-001 (fila do modo noite: Playable também protegido)
[Route("api/playable")]
public sealed class PlayableController : BaseApiController
{
    [HttpGet("status")]
    public IActionResult Status()
    {
        return Success(new
        {
            Available = true
        });
    }
}