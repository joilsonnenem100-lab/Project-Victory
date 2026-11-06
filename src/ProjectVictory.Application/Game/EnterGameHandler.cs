using ProjectVictory.Application.Session;
using ProjectVictory.Application.VerticalSlice;
using ProjectVictory.Domain.Character;
using ProjectVictory.Domain.Runtime;
using ProjectVictory.Application.Runtime;

namespace ProjectVictory.Application.Game;

public sealed class EnterGameHandler
{
    private readonly ICharacterRepository _characterRepository;
    private readonly ICurrentPlayerContext _currentPlayerContext;
    private readonly CreateRuntimeExecutionHandler _createRuntimeExecutionHandler;
    private readonly IRuntimeRepository _runtimeRepository;
    private readonly IRuntimeExecutionStateStore _runtimeExecutionStateStore;
    private readonly ProjectVictory.Application.Progression.ILevelTitleProvider _levelTitles;

    public EnterGameHandler(
        ICharacterRepository characterRepository,
        ICurrentPlayerContext currentPlayerContext,
        CreateRuntimeExecutionHandler createRuntimeExecutionHandler,
        IRuntimeRepository runtimeRepository,
        IRuntimeExecutionStateStore runtimeExecutionStateStore,
        ProjectVictory.Application.Progression.ILevelTitleProvider levelTitles)
    {
        _levelTitles = levelTitles;
        _characterRepository = characterRepository;
        _currentPlayerContext = currentPlayerContext;
        _createRuntimeExecutionHandler = createRuntimeExecutionHandler;
        _runtimeRepository = runtimeRepository;
        _runtimeExecutionStateStore = runtimeExecutionStateStore;
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

        var runtime =
            await _runtimeRepository.GetByCharacterIdAsync(
                character.Id,
                cancellationToken);

        Guid runtimeId;

        if (runtime is not null)
        {
            runtimeId = runtime.RuntimeId;

            _runtimeExecutionStateStore.Store(
                new RuntimeExecutionContext(
                    runtime.RuntimeId,
                    runtime.CharacterId,
                    runtime.Initialized,
                    new CharacterRuntimeState(
                        runtime.Health,
                        runtime.Mana),
                    new RuntimeGameplayContext(
                        RuntimeActivity.Idle),
                    new RuntimeWorldContext(
                        Guid.Empty,
                        Guid.Empty,
                        Guid.Empty)));
        }
        else
        {
            runtimeId = Guid.NewGuid();

            await _createRuntimeExecutionHandler.Handle(
                new CreateRuntimeExecutionCommand(
                    runtimeId,
                    character.Id),
                cancellationToken);

            _runtimeExecutionStateStore.Store(
                new RuntimeExecutionContext(
                    runtimeId,
                    character.Id,
                    true,
                    new CharacterRuntimeState(
                        100,
                        100),
                    new RuntimeGameplayContext(
                        RuntimeActivity.Idle),
                    new RuntimeWorldContext(
                        Guid.Empty,
                        Guid.Empty,
                        Guid.Empty)));
        }

        return new EnterGameResult(
            accountId,
            character.Id,
            runtimeId,
            character.Name,
            character.Level.Value,
            character.Experience.Value,
            character.TitleUnder(_levelTitles.Current).Label,
            character.TitleUnder(_levelTitles.Current).Tier);
    }
}
