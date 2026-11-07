using ProjectVictory.Application.Session;
using ProjectVictory.Domain.Character;

namespace ProjectVictory.Application.Character;

public sealed class GetCharactersHandler
{
    private readonly ICharacterRepository _characterRepository;

    private readonly ICurrentPlayerContext _currentPlayerContext;

    private readonly ProjectVictory.Application.Progression.ILevelTitleProvider _levelTitles;
    private readonly ICharacterProfileStore? _profiles;
    private readonly TimeProvider _clock;



    public GetCharactersHandler(
        ICharacterRepository characterRepository,
        ICurrentPlayerContext currentPlayerContext,
        ProjectVictory.Application.Progression.ILevelTitleProvider levelTitles,
        // PV-CHAR-013 (aditivo e opcional): aparência e exclusão pendente; finaliza as exclusões vencidas.
        ICharacterProfileStore? profiles = null,
        TimeProvider? clock = null)
    {
        _profiles = profiles;
        _clock = clock ?? TimeProvider.System;
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



        IReadOnlyDictionary<Guid, CharacterProfile> profiles = new Dictionary<Guid, CharacterProfile>();

        if (_profiles is not null)
        {
            await _profiles.FinalizeDueDeletionsAsync(accountId, _clock.GetUtcNow(), cancellationToken);
            profiles = await _profiles.GetByAccountAsync(accountId, cancellationToken);
        }

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
                                .ToList(),
                        CharacterClass =
                            entry.Character.CharacterClass.ToString(),
                        Gender =
                            (profiles.GetValueOrDefault(entry.Character.Id)?.Gender ?? ProjectVictory.Domain.Character.CharacterGender.Unspecified).ToString(),
                        Outfit =
                            profiles.GetValueOrDefault(entry.Character.Id)?.Outfit,
                        DeletionScheduledFor =
                            profiles.GetValueOrDefault(entry.Character.Id)?.DeletionScheduledFor
                    })
            .ToList();
    }
}