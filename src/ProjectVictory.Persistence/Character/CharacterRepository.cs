// src/ProjectVictory.Persistence/Character/CharacterRepository.cs

using Microsoft.EntityFrameworkCore;

using ProjectVictory.Domain.Account;
using ProjectVictory.Domain.Character;
using ProjectVictory.Domain.Items;
using ProjectVictory.Domain.ValueObjects;

using DomainCharacter = ProjectVictory.Domain.Entities.Character;

using ProjectVictory.Persistence.Database;

namespace ProjectVictory.Persistence.Character;

public sealed class CharacterRepository : ICharacterRepository
{
    private readonly ProjectVictoryDbContext _context;


    public CharacterRepository(
        ProjectVictoryDbContext context)
    {
        _context = context;
    }



    public async Task AddAsync(
        DomainCharacter character,
        CancellationToken cancellationToken = default)
    {
        var state =
            new PersistentCharacterState
            {
                Id = character.Id,

                AccountId = character.AccountId.Value,

                Name = character.Name,

                CharacterClass = (int)character.CharacterClass,

                Health = character.Health.Value,

                Mana = character.Mana.Value,

                Level = character.Level.Value,

                Experience = character.Experience.Value,

                InventoryItems =
                    character.Inventory.Items
                        .Select(item =>
                            new PersistentItemInstanceState
                            {
                                Id = item.Id,

                                ItemDefinitionId =
                                    item.ItemDefinitionId
                            })
                        .ToList()
            };


        await _context.Characters.AddAsync(
            state,
            cancellationToken);


        await _context.SaveChangesAsync(
            cancellationToken);
    }



    public async Task<DomainCharacter?> GetByIdAsync(
        Guid id,
        CancellationToken cancellationToken = default)
    {
        var state =
            await _context.Characters
                .Include(x => x.InventoryItems)
                .FirstOrDefaultAsync(
                    x => x.Id == id,
                    cancellationToken);


        if (state is null)
        {
            return null;
        }


        return RestoreCharacter(state);
    }



    public async Task<DomainCharacter?> GetByAccountIdAsync(
        Guid accountId,
        CancellationToken cancellationToken = default)
    {
        var state =
            await _context.Characters
                .Include(x => x.InventoryItems)
                .FirstOrDefaultAsync(
                    x => x.AccountId == accountId,
                    cancellationToken);


        if (state is null)
        {
            return null;
        }


        return RestoreCharacter(state);
    }



    public async Task<IReadOnlyList<DomainCharacter>> GetAllByAccountIdAsync(
        Guid accountId,
        CancellationToken cancellationToken = default)
    {
        var states =
            await _context.Characters
                .Include(x => x.InventoryItems)
                .Where(x => x.AccountId == accountId)
                .ToListAsync(
                    cancellationToken);


        return states
            .Select(RestoreCharacter)
            .ToList();
    }



    public async Task<bool> ExistsByNameAsync(
        string name,
        CancellationToken cancellationToken = default)
    {
        return await _context.Characters
            .AnyAsync(
                x => x.Name == name,
                cancellationToken);
    }



    public async Task UpdateAsync(
        DomainCharacter character,
        CancellationToken cancellationToken = default)
    {
        var state =
            await _context.Characters
                .Include(x => x.InventoryItems)
                .FirstOrDefaultAsync(
                    x => x.Id == character.Id,
                    cancellationToken);


        if (state is null)
        {
            return;
        }


        state.Name =
            character.Name;


        state.CharacterClass =
            (int)character.CharacterClass;


        state.Health =
            character.Health.Value;


        state.Mana =
            character.Mana.Value;


        state.Level =
            character.Level.Value;


        state.Experience =
            character.Experience.Value;


        var domainItems =
            character.Inventory.Items
                .ToDictionary(
                    item => item.Id);



        var removedItems =
            state.InventoryItems
                .Where(
                    persisted =>
                        !domainItems.ContainsKey(
                            persisted.Id))
                .ToList();


        foreach (var item in removedItems)
        {
            state.InventoryItems.Remove(
                item);
        }



        var persistedIds =
            state.InventoryItems
                .Select(
                    item => item.Id)
                .ToHashSet();



        var addedItems =
            character.Inventory.Items
                .Where(
                    item =>
                        !persistedIds.Contains(
                            item.Id));



        foreach (var item in addedItems)
        {
            var newItem =
                new PersistentItemInstanceState
                {
                    Id = item.Id,

                    ItemDefinitionId =
                        item.ItemDefinitionId
                };


            state.InventoryItems.Add(
                newItem);


            _context.Entry(newItem).State =
                EntityState.Added;
        }


        Console.WriteLine(
            "===== EF CHANGE TRACKER =====");


        foreach (var entry in _context.ChangeTracker.Entries())
        {
            Console.WriteLine(
                $"Entity: {entry.Entity.GetType().Name} | State: {entry.State}");
        }


        Console.WriteLine(
            "=============================");


        await _context.SaveChangesAsync(
            cancellationToken);
    }



    private static DomainCharacter RestoreCharacter(
        PersistentCharacterState state)
    {
        var characterClass =
            (CharacterClass)state.CharacterClass;


        var definition =
            CharacterClassDefinition.Get(
                characterClass);


        var character =
            new DomainCharacter(
                state.Id,

                new AccountId(
                    state.AccountId),

                state.Name,

                characterClass,

                definition,

                new Health(
                    state.Health),

                new Mana(
                    state.Mana),

                // PV-PROG-010: state.Level não é lido; o Level é derivado da Experience.
                new Experience(
                    state.Experience));


        foreach (var item in state.InventoryItems)
        {
            character.Inventory.AddItem(
                new ItemInstance(
                    item.Id,
                    item.ItemDefinitionId));
        }


        return character;
    }
}