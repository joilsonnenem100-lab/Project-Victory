using ProjectVictory.Application.Session;

using ProjectVictory.Domain.Account;
using ProjectVictory.Domain.Character;
using ProjectVictory.Domain.ValueObjects;

using CharacterEntity = ProjectVictory.Domain.Entities.Character;
using CharacterExperience = ProjectVictory.Domain.ValueObjects.Experience;

namespace ProjectVictory.Application.Character;

public sealed class CreateCharacterHandler
{
    private readonly ProjectVictory.Application.Supply.SupplySettings _supplySettings;

    private readonly ICharacterRepository _characterRepository;

    private readonly ICurrentPlayerContext _currentPlayerContext;


    public CreateCharacterHandler(
        ICharacterRepository characterRepository,
        ICurrentPlayerContext currentPlayerContext,
        ProjectVictory.Application.Supply.SupplySettings supplySettings)
    {
        _supplySettings = supplySettings;
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

                new CharacterExperience(0));



        // PV-SUPPLY-010: poções iniciais por configuração do servidor (PROVISÓRIO; padrão 0).
        for (var i = 0; i < _supplySettings.StarterHealthPotions; i++)
        {
            character.Inventory.AddItem(
                new global::ItemInstance(
                    Guid.NewGuid(),
                    ProjectVictory.Application.Supply.SupplyCatalog.HealthPotionId));
        }

        for (var i = 0; i < _supplySettings.StarterManaPotions; i++)
        {
            character.Inventory.AddItem(
                new global::ItemInstance(
                    Guid.NewGuid(),
                    ProjectVictory.Application.Supply.SupplyCatalog.ManaPotionId));
        }

        await _characterRepository.AddAsync(
            character,
            cancellationToken);


        return character.Id;
    }
}