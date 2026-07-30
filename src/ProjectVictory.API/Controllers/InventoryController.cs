using Microsoft.AspNetCore.Mvc;
using ProjectVictory.Application.Inventory;
using ProjectVictory.Domain.Character;

namespace ProjectVictory.API.Controllers;

[ApiController]
[Route("api/inventory")]
public sealed class InventoryController : ControllerBase
{
    private readonly GetCharacterInventoryHandler _handler;

    private readonly ICharacterRepository _characterRepository;


    public InventoryController(
        GetCharacterInventoryHandler handler,
        ICharacterRepository characterRepository)
    {
        _handler = handler;

        _characterRepository = characterRepository;
    }


    [HttpGet("{characterId:guid}")]
    public async Task<IActionResult> Get(
        Guid characterId,
        CancellationToken cancellationToken)
    {
        var character =
            await _characterRepository.GetByIdAsync(
                characterId,
                cancellationToken);


        if (character is null)
        {
            return NotFound();
        }


        var result =
            _handler.Handle(
                new GetCharacterInventoryQuery(
                    characterId),
                character);


        return Ok(result);
    }
}