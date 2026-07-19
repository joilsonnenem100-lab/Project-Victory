using Microsoft.EntityFrameworkCore;
using ProjectVictory.Domain.Account;
using ProjectVictory.Domain.Character;
using DomainCharacter = ProjectVictory.Domain.Entities.Character;
using ProjectVictory.Domain.ValueObjects;
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
        var state = new PersistentCharacterState
        {
            Id = character.Id,

            AccountId = character.AccountId.Value,

            Name = character.Name,

            Level = character.Level.Value,

            Experience = character.Experience.Value
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


        return RestoreCharacter(state);
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


        return RestoreCharacter(state);
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


        state.Name = character.Name;

        state.Level = character.Level.Value;

        state.Experience =
            character.Experience.Value;


        await _context.SaveChangesAsync(
            cancellationToken);
    }


    private static DomainCharacter RestoreCharacter(
        PersistentCharacterState state)
    {
        return new DomainCharacter(
            state.Id,
            new AccountId(state.AccountId),
            state.Name,
            new Health(100),
            new Mana(100),
            new Experience(state.Experience),
            new Level(state.Level));
    }
}