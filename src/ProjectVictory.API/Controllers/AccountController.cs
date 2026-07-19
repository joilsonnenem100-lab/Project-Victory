using Microsoft.AspNetCore.Mvc;
using ProjectVictory.API.Authentication;
using ProjectVictory.Application.Account;

namespace ProjectVictory.API.Controllers;

[Route("api/account")]
public sealed class AccountController : BaseApiController
{
    private readonly CreateAccountHandler _createHandler;
    private readonly LoginHandler _loginHandler;
    private readonly JwtTokenGenerator _jwtTokenGenerator;


    public AccountController(
        CreateAccountHandler createHandler,
        LoginHandler loginHandler,
        JwtTokenGenerator jwtTokenGenerator)
    {
        _createHandler = createHandler;
        _loginHandler = loginHandler;
        _jwtTokenGenerator = jwtTokenGenerator;
    }


    [HttpPost]
    public async Task<IActionResult> Create(
        CreateAccountCommand command)
    {
        var result =
            await _createHandler.Handle(command);

        return Success(result);
    }


    [HttpPost("login")]
    public async Task<IActionResult> Login(
        LoginCommand command)
    {
        var accountId =
            await _loginHandler.Handle(command);

        var token =
            _jwtTokenGenerator.Generate(accountId);

        return Success(
            new LoginResponse
            {
                AccountId = accountId,
                Token = token
            });
    }
}