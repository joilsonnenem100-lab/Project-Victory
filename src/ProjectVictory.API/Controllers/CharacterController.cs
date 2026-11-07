using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProjectVictory.Application.Character;

namespace ProjectVictory.API.Controllers;

[Authorize]
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


    // PV-CHAR-013 (PO-36/PO-37/PO-57): 5 slots (ocupados, livres e bloqueados) e opções de aparência.
    [HttpGet("slots")]
    public async Task<IActionResult> Slots(
        [FromServices] GetCharacterSlotsHandler slots)
    {
        return Success(await slots.Handle());
    }


    // PV-CHAR-013 (PO-39): pede a exclusão — fica pendente por 7 dias e o servidor finaliza.
    [HttpPost("{characterId:guid}/delete")]
    public async Task<IActionResult> RequestDeletion(
        Guid characterId,
        [FromServices] CharacterDeletionHandler deletion)
    {
        return Success(await deletion.RequestAsync(characterId));
    }


    // PV-CHAR-013 (PROVISÓRIO): cancela a exclusão pendente.
    [HttpPost("{characterId:guid}/cancel-deletion")]
    public async Task<IActionResult> CancelDeletion(
        Guid characterId,
        [FromServices] CharacterDeletionHandler deletion)
    {
        return Success(await deletion.CancelAsync(characterId));
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