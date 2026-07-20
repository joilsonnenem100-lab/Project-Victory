using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProjectVictory.Application.Item;

namespace ProjectVictory.API.Controllers;

[Authorize]
[Route("api/item")]
public sealed class ItemController : BaseApiController
{
    private readonly GetItemDefinitionHandler _handler;


    public ItemController(
        GetItemDefinitionHandler handler)
    {
        _handler = handler;
    }


    [HttpGet("{id:guid}")]
    public async Task<IActionResult> Get(
        Guid id)
    {
        var result =
            await _handler.Handle(
                new GetItemDefinitionQuery(id));


        if (result is null)
        {
            return NotFound();
        }


        return Success(result);
    }
}