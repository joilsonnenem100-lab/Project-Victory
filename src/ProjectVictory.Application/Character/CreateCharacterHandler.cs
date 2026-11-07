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
        // PV-CONTENT-012 (PO-16): kit inicial por nível de poção ("Supplies:StarterKit") + as chaves
        // antigas (nível pequeno), somados em SupplySettings.StarterItems().
        foreach (var (itemDefinitionId, quantity) in _supplySettings.StarterItems())
        {
            for (var i = 0; i < quantity; i++)
            {
                character.Inventory.AddItem(
                    new global::ItemInstance(
                        Guid.NewGuid(),
                        itemDefinitionId));
            }
        }

        await _characterRepository.AddAsync(
            character,
            cancellationToken);


        return character.Id;
    }
}