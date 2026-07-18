using Microsoft.AspNetCore.Mvc;

namespace ProjectVictory.API.Controllers;

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