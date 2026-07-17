using ProjectVictory.Domain.Account;
using ProjectVictory.Domain.Character;
using ProjectVictory.Domain.ValueObjects;

namespace ProjectVictory.Application.Character;

public sealed class CreateCharacterHandler
{
    private readonly ICharacterRepository _characterRepository;

    public CreateCharacterHandler(
        ICharacterRepository characterRepository)
    {
        _characterRepository = characterRepository;
    }

    public async Task<Guid> Handle(
        CreateCharacterCommand command,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(command);

        var accountId = new AccountId(command.AccountId);

        var character = new Domain.Entities.Character(
            Guid.NewGuid(),
            accountId,
            command.Name,
            new Health(100),
            new Mana(50),
            new ProjectVictory.Domain.ValueObjects.Experience(0),
            new Level(1));

        await _characterRepository.AddAsync(
            character,
            cancellationToken);

        return character.Id;
    }
}