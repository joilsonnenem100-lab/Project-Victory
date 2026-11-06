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
                character => (Character: character, Title: character.TitleUnder(titles)))
            .Select(
                entry =>
                    new GetCharactersResult
                    {
                        AccountId =
                            accountId,

                        CharacterId =
                            entry.Character.Id,

                        Name =
                            entry.Character.Name,

                        Level =
                            entry.Character.Level.Value,

                        Experience =
                            entry.Character.Experience.Value,

                        Title =
                            entry.Title.Label,

                        TitleTier =
                            entry.Title.Tier,

                        AvailableAbilities =
                            entry.Character.AvailableAbilities
                                .Select(
                                    ability => ability.Name)
                                .ToList()
                    })
            .ToList();
    }
}