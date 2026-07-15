namespace ProjectVictory.Application.Account;

public sealed record LoginCommand(
    string Email,
    string Password);