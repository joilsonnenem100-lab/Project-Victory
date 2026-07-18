using Microsoft.AspNetCore.Mvc;
using ProjectVictory.Application.Character;

namespace ProjectVictory.API.Controllers;

[Route("api/character")]
public sealed class CharacterController : BaseApiController
{
    private readonly CreateCharacterHandler _createHandler;
    private readonly GetCharactersHandler _getHandler;


    public CharacterController(
        CreateCharacterHandler createHandler,
        GetCharactersHandler getHandler)
    {
        _createHandler = createHandler;
        _getHandler = getHandler;
    }


    [HttpPost]
    public async Task<IActionResult> Create(
        CreateCharacterCommand command)
    {
        var result =
            await _createHandler.Handle(command);

        return Success(result);
    }


    [HttpGet]
    public async Task<IActionResult> Get()
    {
        var result =
            await _getHandler.Handle(
                new GetCharactersQuery());

        return Success(result);
    }
}