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
    private readonly ProjectVictory.Application.GameSession.LoggedOutMarker _loggedOut;

    public EnterGameHandler(
        ICharacterRepository characterRepository,
        ICurrentPlayerContext currentPlayerContext,
        CreateRuntimeExecutionHandler createRuntimeExecutionHandler,
        IRuntimeRepository runtimeRepository,
        IRuntimeExecutionStateStore runtimeExecutionStateStore,
        ProjectVictory.Application.Progression.ILevelTitleProvider levelTitles,
        // PV-RUNTIME-013 (R-054): "entrar no jogo" encerra a marca de logout.
        ProjectVictory.Application.GameSession.LoggedOutMarker loggedOut)
    {
        _loggedOut = loggedOut;
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

            // PV-RUNTIME-012 (fecha o R-055): o contexto em memória (reconstruído pelo Recovery ou vivo
            // desta execução) NÃO é mais sobrescrito; sem ele, usa a MESMA reconstrução do Recovery
            // (tela inicial "Victoria", vida/mana limitadas; vida 0 → cheia, DEC-20261106-48).
            if (!_runtimeExecutionStateStore.Contains(runtime.RuntimeId))
            {
                _runtimeExecutionStateStore.Store(
                    RuntimeReconstruction.FromCheckpoint(runtime, character));
            }

            // PV-RUNTIME-013 (R-054): entrar de novo encerra a marca de logout (memória e checkpoint).
            await _loggedOut.ClearAsync(character.Id, runtime.RuntimeId, cancellationToken);
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

        // Revisão do PV-PROG-013: uma leitura da tabela (rótulo e tier da mesma configuração).
        var title =
            character.TitleUnder(_levelTitles.Current);

        return new EnterGameResult(
            accountId,
            character.Id,
            runtimeId,
            character.Name,
            character.Level.Value,
            character.Experience.Value,
            title.Label,
            title.Tier);
    }
}
