using ProjectVictory.Application.Session;
using ProjectVictory.Domain.Character;

namespace ProjectVictory.Application.Character;

public sealed class GetCharactersHandler
{
    private readonly ICharacterRepository _characterRepository;

    private readonly ICurrentPlayerContext _currentPlayerContext;

    private readonly ProjectVictory.Application.Progression.ILevelTitleProvider _levelTitles;



    public GetCharactersHandler(
        ICharacterRepository characterRepository,
        ICurrentPlayerContext currentPlayerContext,
        ProjectVictory.Application.Progression.ILevelTitleProvider levelTitles)
    {
        _characterRepository = characterRepository;

        _currentPlayerContext = currentPlayerContext;

        _levelTitles = levelTitles;
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



        var titles =
            _levelTitles.Current;

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

                        Title =
                            character.TitleUnder(titles).Label,

                        TitleTier =
                            character.TitleUnder(titles).Tier,

                        AvailableAbilities =
                            character.AvailableAbilities
                                .Select(
                                    ability => ability.Name)
                                .ToList()
                    })
            .ToList();
    }
}