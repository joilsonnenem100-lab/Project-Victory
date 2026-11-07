using Microsoft.EntityFrameworkCore;
using ProjectVictory.Application.HuntAnalyzer;
using ProjectVictory.Domain.Hunt;
using ProjectVictory.Persistence.Database;

namespace ProjectVictory.Persistence.HuntAnalyzer;

// PV-HUD-ANALYZER-010: uma linha por Hunt encerrada (tabela HuntAnalyzerSummaries).
// EndedAtUnixMilliseconds: o SQLite não ordena DateTimeOffset pelo EF Core, por isso o instante é número.
public sealed class PersistentHuntAnalyzerSummaryState
{
    public Guid HuntId { get; set; }

    public Guid CharacterId { get; set; }

    public string EndReason { get; set; } = string.Empty;

    public long EndedAtUnixMilliseconds { get; set; }

    public long ExperienceGained { get; set; }

    public long ElapsedSeconds { get; set; }

    public long ExperiencePerHour { get; set; }

    public long Debit { get; set; }

    public long LootValue { get; set; }

    public long Gold { get; set; }

    public long Profit { get; set; }

    public long NetProfit { get; set; }

    public int SuppliesUsed { get; set; }

    public int ItemsLooted { get; set; }
}

// PV-HUD-ANALYZER-010 (PO-47): histórico das últimas Hunts (Permanente). Grava uma vez, no fim da Hunt
// (assinante do HuntEndedEvent), e apaga só as linhas do próprio personagem além das N mais recentes.
public sealed class HuntAnalyzerHistoryStore : IHuntAnalyzerHistoryStore
{
    private readonly ProjectVictoryDbContext _context;

    public HuntAnalyzerHistoryStore(ProjectVictoryDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(HuntAnalyzerSummary summary, int keep, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(summary);
        ArgumentOutOfRangeException.ThrowIfLessThan(keep, 1);

        var existing = await _context.HuntAnalyzerSummaries.FindAsync([summary.HuntId], cancellationToken);
        var row = existing ?? new PersistentHuntAnalyzerSummaryState { HuntId = summary.HuntId };

        row.CharacterId = summary.CharacterId;
        row.EndReason = summary.EndReason;
        row.EndedAtUnixMilliseconds = summary.EndedAt.ToUnixTimeMilliseconds();
        row.ExperienceGained = summary.Report.ExperienceGained;
        row.ElapsedSeconds = summary.Report.ElapsedSeconds;
        row.ExperiencePerHour = summary.Report.ExperiencePerHour;
        row.Debit = summary.Report.Debit;
        row.LootValue = summary.Report.LootValue;
        row.Gold = summary.Report.Gold;
        row.Profit = summary.Report.Profit;
        row.NetProfit = summary.Report.NetProfit;
        row.SuppliesUsed = summary.Report.SuppliesUsed;
        row.ItemsLooted = summary.Report.ItemsLooted;

        if (existing is null)
        {
            _context.HuntAnalyzerSummaries.Add(row);
        }

        await _context.SaveChangesAsync(cancellationToken);

        var surplus = await _context.HuntAnalyzerSummaries
            .Where(x => x.CharacterId == summary.CharacterId)
            .OrderByDescending(x => x.EndedAtUnixMilliseconds)
            .Skip(keep)
            .ToListAsync(cancellationToken);

        if (surplus.Count > 0)
        {
            _context.HuntAnalyzerSummaries.RemoveRange(surplus);
            await _context.SaveChangesAsync(cancellationToken);
        }
    }

    public async Task<IReadOnlyList<HuntAnalyzerSummary>> GetRecentAsync(Guid characterId, int count, CancellationToken cancellationToken = default)
    {
        if (count < 1)
        {
            return [];
        }

        var rows = await _context.HuntAnalyzerSummaries
            .AsNoTracking()
            .Where(x => x.CharacterId == characterId)
            .OrderByDescending(x => x.EndedAtUnixMilliseconds)
            .Take(count)
            .ToListAsync(cancellationToken);

        return rows.Select(row => new HuntAnalyzerSummary(
                row.HuntId,
                row.CharacterId,
                row.EndReason,
                DateTimeOffset.FromUnixTimeMilliseconds(row.EndedAtUnixMilliseconds),
                new HuntAnalyzerReport(
                    row.ExperienceGained,
                    row.ElapsedSeconds,
                    row.ExperiencePerHour,
                    row.Debit,
                    row.LootValue,
                    row.Gold,
                    row.Profit,
                    row.NetProfit,
                    row.SuppliesUsed,
                    row.ItemsLooted)))
            .ToList();
    }
}
