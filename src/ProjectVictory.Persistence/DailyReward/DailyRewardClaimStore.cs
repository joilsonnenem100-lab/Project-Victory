using Microsoft.EntityFrameworkCore;
using ProjectVictory.Application.DailyReward;
using ProjectVictory.Persistence.Database;

namespace ProjectVictory.Persistence.DailyReward;

// PV-DAILY-010 (PO-60): um registro por conta e ciclo diário (tabela DailyRewardClaims, chave
// AccountId + CycleStartUnixMilliseconds). Instantes como número no SQLite (padrão do HuntAnalyzer).
public sealed class PersistentDailyRewardClaimState
{
    public Guid AccountId { get; set; }

    public long CycleStartUnixMilliseconds { get; set; }

    public long ClaimedAtUnixMilliseconds { get; set; }

    public Guid CharacterId { get; set; }
}

public sealed class DailyRewardClaimStore : IDailyRewardClaimStore
{
    private readonly ProjectVictoryDbContext _context;

    public DailyRewardClaimStore(ProjectVictoryDbContext context)
    {
        _context = context;
    }

    public async Task<DailyRewardClaim?> GetLastAsync(Guid accountId, CancellationToken cancellationToken = default)
    {
        var row = await _context.DailyRewardClaims
            .AsNoTracking()
            .Where(x => x.AccountId == accountId)
            .OrderByDescending(x => x.CycleStartUnixMilliseconds)
            .FirstOrDefaultAsync(cancellationToken);

        return row is null
            ? null
            : new DailyRewardClaim(
                row.AccountId,
                DateTimeOffset.FromUnixTimeMilliseconds(row.CycleStartUnixMilliseconds),
                DateTimeOffset.FromUnixTimeMilliseconds(row.ClaimedAtUnixMilliseconds),
                row.CharacterId);
    }

    public async Task<bool> TryAddAsync(DailyRewardClaim claim, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(claim);

        var cycle = claim.CycleStart.ToUnixTimeMilliseconds();

        if (await _context.DailyRewardClaims.AnyAsync(
                x => x.AccountId == claim.AccountId && x.CycleStartUnixMilliseconds == cycle,
                cancellationToken))
        {
            return false;
        }

        var row = new PersistentDailyRewardClaimState
        {
            AccountId = claim.AccountId,
            CycleStartUnixMilliseconds = cycle,
            ClaimedAtUnixMilliseconds = claim.ClaimedAt.ToUnixTimeMilliseconds(),
            CharacterId = claim.CharacterId
        };

        _context.DailyRewardClaims.Add(row);

        try
        {
            await _context.SaveChangesAsync(cancellationToken);
            return true;
        }
        catch (DbUpdateException)
        {
            // Outro processo gravou o mesmo ciclo (chave primária): não entrega de novo.
            _context.Entry(row).State = EntityState.Detached;
            return false;
        }
    }
}
