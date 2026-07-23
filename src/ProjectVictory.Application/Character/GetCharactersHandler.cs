using ProjectVictory.Application.Session;
using ProjectVictory.Domain.Character;

namespace ProjectVictory.Application.Character;

public sealed class GetCharactersHandler
{
    private readonly ICharacterRepository _characterRepository;

    private readonly ICurrentPlayerContext _currentPlayerContext;



    public GetCharactersHandler(
        ICharacterRepository characterRepository,
        ICurrentPlayerContext currentPlayerContext)
    {
        _characterRepository = characterRepository;

        _currentPlayerContext = currentPlayerContext;
    }



    public async Task<IReadOnlyList<GetCharactersResult>> Handle(
        GetCharactersQuery query,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(query);



        var accountId =
            _currentPlayerContext.AccountId;



        var characters =
            await _characterRepository.GetAllByAccountIdAsync(
                accountId,
                cancellationToken);



        return characters
            .Select(
                character =>
                    new GetCharactersResult
                    {
                        AccountId =
                            accountId,

                        CharacterId =
                            character.Id,

                        Name =
                            character.Name,

                        Level =
                            character.Level.Value,

                        Experience =
                            character.Experience.Value,

                        AvailableAbilities =
                            character.AvailableAbilities
                                .Select(
                                    ability => ability.Name)
                                .ToList()
                    })
            .ToList();
    }
}