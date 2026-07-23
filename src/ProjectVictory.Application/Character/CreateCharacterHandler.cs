using ProjectVictory.Application.Session;

using ProjectVictory.Domain.Account;
using ProjectVictory.Domain.Character;
using ProjectVictory.Domain.ValueObjects;

using CharacterEntity = ProjectVictory.Domain.Entities.Character;
using CharacterExperience = ProjectVictory.Domain.ValueObjects.Experience;

namespace ProjectVictory.Application.Character;

public sealed class CreateCharacterHandler
{
    private readonly ICharacterRepository _characterRepository;

    private readonly ICurrentPlayerContext _currentPlayerContext;


    public CreateCharacterHandler(
        ICharacterRepository characterRepository,
        ICurrentPlayerContext currentPlayerContext)
    {
        _characterRepository =
            characterRepository;

        _currentPlayerContext =
            currentPlayerContext;
    }


    public async Task<Guid> Handle(
        CreateCharacterCommand command,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(command);


        var definition =
            CharacterClassDefinition.Get(
                command.CharacterClass);



        var character =
            new CharacterEntity(
                Guid.NewGuid(),

                new AccountId(
                    _currentPlayerContext.AccountId),

                command.Name,

                command.CharacterClass,

                definition,

                new Health(
                    definition.InitialHealth),

                new Mana(
                    definition.InitialMana),

                new CharacterExperience(0),

                new Level(1));



        await _characterRepository.AddAsync(
            character,
            cancellationToken);


        return character.Id;
    }
}