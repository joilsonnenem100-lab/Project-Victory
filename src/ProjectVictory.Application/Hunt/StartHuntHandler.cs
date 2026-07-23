using ProjectVictory.Domain.Character;
using ProjectVictory.Domain.Hunt;

namespace ProjectVictory.Application.Hunt;

public sealed class StartHuntHandler
{
    private readonly ICharacterRepository _characterRepository;

    private readonly IHuntRepository _huntRepository;


    public StartHuntHandler(
        ICharacterRepository characterRepository,
        IHuntRepository huntRepository)
    {
        _characterRepository = characterRepository;

        _huntRepository = huntRepository;
    }


    public async Task<StartHuntResult> Handle(
        StartHuntCommand command,
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


        var configuration =
            new HuntConfiguration(
                command.EnemyId);


        var hunt =
            new Domain.Hunt.Hunt(
                Guid.NewGuid(),
                character.Id,
                command.RuntimeId);


        hunt.Start();


        await _huntRepository.AddAsync(
            hunt,
            cancellationToken);


        return new StartHuntResult(
            hunt.Id,
            character.Id);
    }
}