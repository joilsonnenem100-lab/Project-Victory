using ProjectVictory.Domain.Character;
using ProjectVictory.Domain.Hunt;

namespace ProjectVictory.Application.Hunt;

public sealed class StartHuntSessionHandler
{
    private readonly ICharacterRepository _characterRepository;
    private readonly IHuntSessionRepository _repository;


    public StartHuntSessionHandler(
        ICharacterRepository characterRepository,
        IHuntSessionRepository repository)
    {
        _characterRepository = characterRepository;
        _repository = repository;
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

        return new StartHuntSessionResult(
            session.Id,
            character.Id);
    }
}
