using ProjectVictory.Application.Session;
using ProjectVictory.Domain.Account;
using ProjectVictory.Domain.Character;
using ProjectVictory.Domain.ValueObjects;

namespace ProjectVictory.Application.Character;

public sealed class CreateCharacterHandler
{
    private readonly ICharacterRepository _characterRepository;
    private readonly ICurrentPlayerContext _currentPlayerContext;


    public CreateCharacterHandler(
        ICharacterRepository characterRepository,
        ICurrentPlayerContext currentPlayerContext)
    {
        _characterRepository = characterRepository;
        _currentPlayerContext = currentPlayerContext;
    }


    public async Task<Guid> Handle(
        CreateCharacterCommand command,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(command);


        var accountId =
            _currentPlayerContext.AccountId;


        var existingCharacter =
            await _characterRepository.GetByAccountIdAsync(
                accountId,
                cancellationToken);


        if (existingCharacter is not null)
        {
            throw new InvalidOperationException(
                "Character already exists for this account.");
        }


        var character =
            new Domain.Entities.Character(
                Guid.NewGuid(),
                new AccountId(accountId),
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