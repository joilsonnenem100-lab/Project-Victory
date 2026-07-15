using ProjectVictory.Domain.Account;

namespace ProjectVictory.Domain.Game;

public sealed class GameSession
{
    public AccountId AccountId { get; }

    public GameSession(AccountId accountId)
    {
        ArgumentNullException.ThrowIfNull(accountId);

        AccountId = accountId;
    }
}