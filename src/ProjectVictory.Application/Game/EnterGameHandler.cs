using ProjectVictory.Domain.Character;

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

        var accountId = new ProjectVictory.Domain.Account.AccountId(
            command.AccountId);

        var character = await _characterRepository.GetByAccountIdAsync(
            accountId.Value,
            cancellationToken);

        if (character is null)
        {
            throw new InvalidOperationException(
                "No character found for account.");
        }

        return new EnterGameResult(
            command.AccountId,
            character.Id);
    }
}