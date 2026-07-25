using Microsoft.EntityFrameworkCore;
using ProjectVictory.Domain.Hunt;
using ProjectVictory.Persistence.Database;

namespace ProjectVictory.Persistence.Hunt;

public sealed class HuntSessionRepository : IHuntSessionRepository
{
    private readonly ProjectVictoryDbContext _context;

    public HuntSessionRepository(
        ProjectVictoryDbContext context)
    {
        _context = context;
    }


    public async Task AddAsync(
        HuntSession session,
        CancellationToken cancellationToken = default)
    {
        var state = new PersistentHuntSessionState
        {
            Id = session.Id,
            CharacterId = session.CharacterId,
            Status = session.Status.ToString(),
            CreatedAt = session.CreatedAt,
            StartedAt = session.StartedAt,
            StoppedAt = session.StoppedAt
        };

        await _context.HuntSessions.AddAsync(
            state,
            cancellationToken);

        await _context.SaveChangesAsync(
            cancellationToken);
    }


    public async Task<HuntSession?> GetByIdAsync(
        Guid id,
        CancellationToken cancellationToken = default)
    {
        var state =
            await _context.HuntSessions
                .FirstOrDefaultAsync(
                    x => x.Id == id,
                    cancellationToken);

        if (state is null)
        {
            return null;
        }

        return Restore(state);
    }


    private static HuntSession Restore(
        PersistentHuntSessionState state)
    {
        var session =
            new HuntSession(
                state.Id,
                state.CharacterId);

        if (state.Status == HuntSessionStatus.Running.ToString())
        {
            session.Start();
        }

        if (state.Status == HuntSessionStatus.Stopped.ToString())
        {
            session.Start();
            session.Stop();
        }

        return session;
    }
}
