using Microsoft.AspNetCore.Mvc;
using ProjectVictory.Application.Runtime;
using ProjectVictory.Application.VerticalSlice;

namespace ProjectVictory.API.Controllers;

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
    public IActionResult Create(
        [FromBody] CreateRuntimeExecutionCommand command)
    {
        var result =
            _createHandler.Handle(command);

        return Success(result);
    }

    [HttpPost("activity")]
    public IActionResult UpdateActivity(
        [FromBody] UpdateRuntimeActivityCommand command)
    {
        var result =
            _activityHandler.Handle(command);

        return Success(result);
    }

    [HttpPost("location")]
    public IActionResult ChangeLocation(
        [FromBody] ChangeRuntimeLocationCommand command)
    {
        var result =
            _locationHandler.Handle(command);

        return Success(result);
    }
}
