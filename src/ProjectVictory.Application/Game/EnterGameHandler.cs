using ProjectVictory.Application.Session;
using ProjectVictory.Application.VerticalSlice;
using ProjectVictory.Domain.Character;

namespace ProjectVictory.Application.Game;

public sealed class EnterGameHandler
{
    private readonly ICharacterRepository _characterRepository;
    private readonly ICurrentPlayerContext _currentPlayerContext;
    private readonly CreateRuntimeExecutionHandler _createRuntimeExecutionHandler;

    public EnterGameHandler(
        ICharacterRepository characterRepository,
        ICurrentPlayerContext currentPlayerContext,
        CreateRuntimeExecutionHandler createRuntimeExecutionHandler)
    {
        _characterRepository = characterRepository;
        _currentPlayerContext = currentPlayerContext;
        _createRuntimeExecutionHandler = createRuntimeExecutionHandler;
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

        var runtimeId =
            Guid.NewGuid();

        await _createRuntimeExecutionHandler.Handle(
            new CreateRuntimeExecutionCommand(
                runtimeId,
                character.Id),
            cancellationToken);

        return new EnterGameResult(
            accountId,
            character.Id,
            runtimeId,
            character.Name,
            character.Level.Value,
            character.Experience.Value);
    }
}