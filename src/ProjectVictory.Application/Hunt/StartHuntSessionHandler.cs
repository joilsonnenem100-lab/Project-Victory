using ProjectVictory.Application.VerticalSlice;
using ProjectVictory.Domain.Character;
using ProjectVictory.Domain.Hunt;
using ProjectVictory.Domain.Runtime;

namespace ProjectVictory.Application.Hunt;

public sealed class StartHuntSessionHandler
{
    private readonly ICharacterRepository _characterRepository;
    private readonly IHuntSessionRepository _repository;
    private readonly IRuntimeExecutionStateStore _runtimeExecutionStateStore;

    public StartHuntSessionHandler(
        ICharacterRepository characterRepository,
        IHuntSessionRepository repository,
        IRuntimeExecutionStateStore runtimeExecutionStateStore)
    {
        _characterRepository = characterRepository;
        _repository = repository;
        _runtimeExecutionStateStore = runtimeExecutionStateStore;
    }

    public async Task<StartHuntSessionResult> Handle(
        StartHuntSessionCommand command,
        CancellationToken cancellationToken = default)
    {
        var character =
            await _characterRepository.GetByIdAsync(
                command.CharacterId,
                cancellationToken);

        if (character is null)
        {
            throw new InvalidOperationException(
                "Character not found.");
        }

        var session =
            new HuntSession(
                Guid.NewGuid(),
                character.Id);

        session.Start();

        await _repository.AddAsync(
            session,
            cancellationToken);

        var runtimeContext =
            _runtimeExecutionStateStore.GetByCharacterId(
                character.Id);

        _runtimeExecutionStateStore.Store(
            runtimeContext with
            {
                GameplayContext =
                    new RuntimeGameplayContext(
                        RuntimeActivity.Hunting,
                        session.Id,
                        runtimeContext.GameplayContext?.CurrentEncounterId,
                        runtimeContext.GameplayContext?.TemporaryState)
            });

        return new StartHuntSessionResult(
            session.Id,
            character.Id);
    }
}
