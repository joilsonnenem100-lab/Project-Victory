using Microsoft.AspNetCore.Mvc;
using ProjectVictory.Application.Account;

namespace ProjectVictory.API.Controllers;

[Route("api/account")]
public sealed class AccountController : BaseApiController
{
    private readonly CreateAccountHandler _handler;

    public AccountController(CreateAccountHandler handler)
    {
        _handler = handler;
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateAccountCommand command)
    {
        var result = await _handler.Handle(command);

        return Success(result);
    }
}