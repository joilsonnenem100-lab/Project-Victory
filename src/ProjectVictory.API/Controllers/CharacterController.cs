using Microsoft.AspNetCore.Mvc;
using ProjectVictory.Application.Character;

namespace ProjectVictory.API.Controllers;

[Route("api/character")]
public sealed class CharacterController : BaseApiController
{
    private readonly CreateCharacterHandler _handler;

    public CharacterController(CreateCharacterHandler handler)
    {
        _handler = handler;
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateCharacterCommand command)
    {
        var result = await _handler.Handle(command);

        return Success(result);
    }
}