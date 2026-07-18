using ProjectVictory.Application.Session;
using ProjectVictory.Domain.Character;

namespace ProjectVictory.Application.Game;

public sealed class EnterGameHandler
{
    private readonly ICharacterRepository _characterRepository;
    private readonly ICurrentPlayerContext _currentPlayerContext;


    public EnterGameHandler(
        ICharacterRepository characterRepository,
        ICurrentPlayerContext currentPlayerContext)
    {
        _characterRepository = characterRepository;
        _currentPlayerContext = currentPlayerContext;
    }


    public async Task<EnterGameResult> Handle(
        EnterGameCommand command,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(command);


        var accountId =
            _currentPlayerContext.AccountId;


        var character =
            await _characterRepository.GetByAccountIdAsync(
                accountId,
                cancellationToken);


        if (character is null)
        {
            throw new InvalidOperationException(
                "No character found for account.");
        }


        return new EnterGameResult(
            accountId,
            character.Id,
            character.Level.Value,
            character.Experience.Value);
    }
}