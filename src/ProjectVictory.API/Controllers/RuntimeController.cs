using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProjectVictory.Application.Runtime;
using ProjectVictory.Application.VerticalSlice;

namespace ProjectVictory.API.Controllers;

[Authorize] // PV-SEC-001
[Route("api/runtime")]
public sealed class RuntimeController : BaseApiController
{
    private readonly CreateRuntimeExecutionHandler _createHandler;
    private readonly UpdateRuntimeActivityHandler _activityHandler;
    private readonly ChangeRuntimeLocationHandler _locationHandler;

    public RuntimeController(
        CreateRuntimeExecutionHandler createHandler,
        UpdateRuntimeActivityHandler activityHandler,
        ChangeRuntimeLocationHandler locationHandler)
    {
        _createHandler = createHandler;
        _activityHandler = activityHandler;
        _locationHandler = locationHandler;
    }

    [HttpPost("create")]
    public async Task<IActionResult> Create(
        [FromBody] CreateRuntimeExecutionCommand command)
    {
        // PV-SEC-001 (D3): antes o Task era serializado sem await.
        var result =
            await _createHandler.Handle(command);

        return Success(result);
    }

    [HttpPost("activity")]
    public async Task<IActionResult> UpdateActivity(
        [FromBody] UpdateRuntimeActivityCommand command)
    {
        var result =
            await _activityHandler.Handle(command);

        return Success(result);
    }

    [HttpPost("location")]
    public async Task<IActionResult> ChangeLocation(
        [FromBody] ChangeRuntimeLocationCommand command)
    {
        var result =
            await _locationHandler.Handle(command);

        return Success(result);
    }
}
