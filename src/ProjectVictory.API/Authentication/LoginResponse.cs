namespace ProjectVictory.API.Authentication;

public sealed class LoginResponse
{
    public Guid AccountId { get; init; }

    public string Token { get; init; } = string.Empty;
}