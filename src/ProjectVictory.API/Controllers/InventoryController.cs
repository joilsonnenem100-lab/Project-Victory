using Microsoft.AspNetCore.Mvc;
using ProjectVictory.Application.Inventory;
using ProjectVictory.Domain.Entities;

namespace ProjectVictory.API.Controllers;

[ApiController]
[Route("api/inventory")]
public class InventoryController : ControllerBase
{
    private readonly GetCharacterInventoryHandler _handler;

    public InventoryController(
        GetCharacterInventoryHandler handler)
    {
        _handler = handler;
    }


    [HttpGet("{characterId:guid}")]
    public IActionResult Get(
        Guid characterId)
    {
        // Endpoint preparado para integração com o Character existente.
        // A resolução do Character deve seguir o fluxo existente de persistência.

        return Ok(
            new InventoryResult(
                true,
                Array.Empty<string>()
            )
        );
    }
}