using ProjectVictory.Application.Hunt;
using ProjectVictory.Application.VerticalSlice;
using ProjectVictory.Domain.Character;
using ProjectVictory.Domain.Hunt;

namespace ProjectVictory.Application.Session;

// PV-SEC-001: verifica se o recurso pedido pertence à conta autenticada.
// A identidade vem sempre de ICurrentPlayerContext (token), nunca do corpo da requisição.
public sealed class PlayerOwnershipGuard
{
    private readonly ICurrentPlayerContext _currentPlayerContext;
    private readonly ICharacterRepository _characterRepository;
    private readonly IRuntimeExecutionStateStore _runtimeExecutionStateStore;
    private readonly IHuntSessionRepository _huntSessionRepository;
    private readonly IEncounterStateStore _encounterStateStore;
    private readonly IHuntRepository _huntRepository;


    public PlayerOwnershipGuard(
        ICurrentPlayerContext currentPlayerContext,
        ICharacterRepository characterRepository,
        IRuntimeExecutionStateStore runtimeExecutionStateStore,
        IHuntSessionRepository huntSessionRepository,
        IEncounterStateStore encounterStateStore,
        IHuntRepository huntRepository)
    {
        _currentPlayerContext = currentPlayerContext;
        _characterRepository = characterRepository;
        _runtimeExecutionStateStore = runtimeExecutionStateStore;
        _huntSessionRepository = huntSessionRepository;
        _encounterStateStore = encounterStateStore;
        _huntRepository = huntRepository;
    }


    // Personagem inexistente também é negado (não revela quais ids existem).
    public async Task EnsureCharacterAsync(
        Guid characterId,
        CancellationToken cancellationToken = default)
    {
        var character =
            await _characterRepository.GetByIdAsync(
                characterId,
                cancellationToken);

        if (character is null
            || character.AccountId.Value != _currentPlayerContext.AccountId)
        {
            throw new OwnershipViolationException(
                "character");
        }
    }


    // Runtime inexistente mantém o comportamento atual do store.
    public Task EnsureRuntimeAsync(
        Guid runtimeId,
        CancellationToken cancellationToken = default)
    {
        var runtime =
            _runtimeExecutionStateStore.Get(
                runtimeId);

        return EnsureCharacterAsync(
            runtime.CharacterId,
            cancellationToken);
    }


    public async Task EnsureHuntSessionAsync(
        Guid huntSessionId,
        CancellationToken cancellationToken = default)
    {
        var session =
            await _huntSessionRepository.GetByIdAsync(
                huntSessionId,
                cancellationToken);

        if (session is null)
        {
            throw new OwnershipViolationException(
                "hunt session");
        }

        await EnsureCharacterAsync(
            session.CharacterId,
            cancellationToken);
    }


    // Encounter inexistente mantém o comportamento atual do store.
    public async Task EnsureEncounterAsync(
        Guid encounterId,
        CancellationToken cancellationToken = default)
    {
        var encounter =
            _encounterStateStore.Get(
                encounterId);

        var hunt =
            await _huntRepository.GetByIdAsync(
                encounter.HuntId,
                cancellationToken);

        if (hunt is null)
        {
            throw new OwnershipViolationException(
                "encounter");
        }

        await EnsureCharacterAsync(
            hunt.CharacterId,
            cancellationToken);
    }
}
