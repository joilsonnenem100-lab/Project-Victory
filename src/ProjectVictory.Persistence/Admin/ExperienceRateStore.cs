using ProjectVictory.Application.Reward;
using ProjectVictory.Persistence.Database;

namespace ProjectVictory.Persistence.Admin;

// PV-ADMIN-010: XP rate alterado pelo administrador (tabela ServerExperienceRate, linha única Id = 1).
// ChangedAtUnixMilliseconds: mesmo padrão do HuntAnalyzer (instante como número no SQLite).
public sealed class PersistentServerExperienceRateState
{
    public const int SingletonId = 1;

    public int Id { get; set; } = SingletonId;

    public decimal Value { get; set; }

    public Guid ChangedByAccountId { get; set; }

    public long ChangedAtUnixMilliseconds { get; set; }
}

// PV-ADMIN-010 (DEC-20261106-12): persistência do XP rate do servidor (Permanente).
public sealed class ExperienceRateStore : IExperienceRateStore
{
    private readonly ProjectVictoryDbContext _context;

    public ExperienceRateStore(ProjectVictoryDbContext context)
    {
        _context = context;
    }

    public async Task<ExperienceRateOverride?> GetAsync(CancellationToken cancellationToken = default)
    {
        var row = await _context.ServerExperienceRate.FindAsync(
            [PersistentServerExperienceRateState.SingletonId],
            cancellationToken);

        return row is null
            ? null
            : new ExperienceRateOverride(
                row.Value,
                row.ChangedByAccountId,
                DateTimeOffset.FromUnixTimeMilliseconds(row.ChangedAtUnixMilliseconds));
    }

    public async Task SaveAsync(ExperienceRateOverride value, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(value);

        var row = await _context.ServerExperienceRate.FindAsync(
            [PersistentServerExperienceRateState.SingletonId],
            cancellationToken);

        if (row is null)
        {
            row = new PersistentServerExperienceRateState();
            _context.ServerExperienceRate.Add(row);
        }

        row.Value = value.Value;
        row.ChangedByAccountId = value.ChangedByAccountId;
        row.ChangedAtUnixMilliseconds = value.ChangedAt.ToUnixTimeMilliseconds();

        await _context.SaveChangesAsync(cancellationToken);
    }
}
