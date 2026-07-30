using ProjectVictory.Application.VerticalSlice;
using Microsoft.EntityFrameworkCore;
using ProjectVictory.Persistence.Database;

namespace ProjectVictory.Persistence.Runtime;

public sealed class RuntimeRepository : IRuntimeRepository
{
    private readonly ProjectVictoryDbContext _context;

    public RuntimeRepository(
        ProjectVictoryDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(
        Guid runtimeId,
        Guid characterId,
        int health,
        int mana,
        CancellationToken cancellationToken = default)
    {
        await _context.RuntimeStates.AddAsync(
            new PersistentRuntimeState
            {
                Id = runtimeId,
                RuntimeId = runtimeId,
                CharacterId = characterId,
                Initialized = true,
                Health = health,
                Mana = mana
            },
            cancellationToken);

        await _context.SaveChangesAsync(
            cancellationToken);
    }

    public async Task<RuntimeRecoveryData?> GetByCharacterIdAsync(
        Guid characterId,
        CancellationToken cancellationToken = default)
    {
        var state =
            await _context.RuntimeStates
                .AsNoTracking()
                .FirstOrDefaultAsync(
                    x => x.CharacterId == characterId,
                    cancellationToken);

        if (state is null)
        {
            return null;
        }

        return new RuntimeRecoveryData(
            state.RuntimeId,
            state.CharacterId,
            state.Initialized,
            state.Health,
            state.Mana);
    }
}
