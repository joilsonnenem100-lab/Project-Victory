namespace ProjectVictory.Application.Account;

public sealed record CreateAccountCommand(
    string Email,
    string Password);