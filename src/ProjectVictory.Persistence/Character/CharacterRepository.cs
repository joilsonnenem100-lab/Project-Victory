// src/ProjectVictory.Persistence/Character/CharacterRepository.cs

using Microsoft.EntityFrameworkCore;

using ProjectVictory.Domain.Account;
using ProjectVictory.Domain.Character;
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
                Id =
                    character.Id,

                AccountId =
                    character.AccountId.Value,

                Name =
                    character.Name,

                CharacterClass =
                    (int)character.CharacterClass,

                Health =
                    character.Health.Value,

                Mana =
                    character.Mana.Value,

                Level =
                    character.Level.Value,

                Experience =
                    character.Experience.Value
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
                .FirstOrDefaultAsync(
                    x => x.Id == id,
                    cancellationToken);


        if (state is null)
        {
            return null;
        }


        return RestoreCharacter(
            state);
    }



    public async Task<DomainCharacter?> GetByAccountIdAsync(
        Guid accountId,
        CancellationToken cancellationToken = default)
    {
        var state =
            await _context.Characters
                .FirstOrDefaultAsync(
                    x => x.AccountId == accountId,
                    cancellationToken);


        if (state is null)
        {
            return null;
        }


        return RestoreCharacter(
            state);
    }



    public async Task<IReadOnlyList<DomainCharacter>> GetAllByAccountIdAsync(
        Guid accountId,
        CancellationToken cancellationToken = default)
    {
        var states =
            await _context.Characters
                .Where(
                    x => x.AccountId == accountId)
                .ToListAsync(
                    cancellationToken);


        return states
            .Select(
                RestoreCharacter)
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


        return new DomainCharacter(
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

            new Experience(
                state.Experience),

            new Level(
                state.Level));
    }
}