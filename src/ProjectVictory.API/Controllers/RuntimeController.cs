using Microsoft.AspNetCore.Mvc;
using ProjectVictory.Application.VerticalSlice;

namespace ProjectVictory.API.Controllers;

[Route("api/runtime")]
public sealed class RuntimeController : BaseApiController
{
    private readonly CreateRuntimeExecutionHandler _handler;


    public RuntimeController(
        CreateRuntimeExecutionHandler handler)
    {
        _handler = handler;
    }


    [HttpPost("create")]
    public IActionResult Create(
        [FromBody] CreateRuntimeExecutionCommand command)
    {
        var result =
            _handler.Handle(
                command);

        return Success(
            result);
    }
}