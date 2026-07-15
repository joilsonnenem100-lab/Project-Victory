using ProjectVictory.Domain.Account;
using ProjectVictory.Domain.Character;
using ProjectVictory.Domain.Game;

namespace ProjectVictory.Application.Game;

public sealed class EnterGameHandler
{
    private readonly ICharacterRepository _characterRepository;

    public EnterGameHandler(
        ICharacterRepository characterRepository)
    {
        _characterRepository = characterRepository;
    }

    public async Task<EnterGameResult> Handle(
        EnterGameCommand command,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(command);

        var accountId = new AccountId(command.AccountId);

        var character = await _characterRepository
            .GetByAccountIdAsync(
                accountId,
                cancellationToken);

        if (character is null)
        {
            throw new InvalidOperationException(
                "No character available for account.");
        }

        var session = new GameSession(accountId);

        return new EnterGameResult(
            session.AccountId.Value,
            character.Id);
    }
}