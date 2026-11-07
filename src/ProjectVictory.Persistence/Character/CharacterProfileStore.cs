using Microsoft.EntityFrameworkCore;
using ProjectVictory.Application.Character;
using ProjectVictory.Domain.Character;
using ProjectVictory.Persistence.Database;

namespace ProjectVictory.Persistence.Character;

// PV-CHAR-013 (PO-37/PO-38/PO-39): aparência e exclusão do personagem, nas colunas aditivas da tabela
// Characters. A exclusão finalizada só marca DeletedAt (o histórico do personagem fica no banco).
public sealed class CharacterProfileStore : ICharacterProfileStore
{
    private readonly ProjectVictoryDbContext _context;

    public CharacterProfileStore(ProjectVictoryDbContext context)
    {
        _context = context;
    }

    public async Task<IReadOnlyDictionary<Guid, CharacterProfile>> GetByAccountAsync(
        Guid accountId,
        CancellationToken cancellationToken = default)
    {
        var rows = await _context.Characters
            .AsNoTracking()
            .Where(x => x.AccountId == accountId && x.DeletedAtUnixMilliseconds == null)
            .Select(x => new
            {
                x.Id,
                x.Gender,
                x.Outfit,
                x.DeletionRequestedAtUnixMilliseconds,
                x.DeletionScheduledForUnixMilliseconds
            })
            .ToListAsync(cancellationToken);

        return rows.ToDictionary(
            x => x.Id,
            x => new CharacterProfile(
                x.Id,
                (CharacterGender)x.Gender,
                x.Outfit,
                FromUnix(x.DeletionRequestedAtUnixMilliseconds),
                FromUnix(x.DeletionScheduledForUnixMilliseconds)));
    }

    public async Task SetAppearanceAsync(
        Guid characterId,
        CharacterGender gender,
        string outfit,
        CancellationToken cancellationToken = default)
    {
        var row = await _context.Characters.FirstOrDefaultAsync(x => x.Id == characterId, cancellationToken);

        if (row is null)
        {
            return;
        }

        row.Gender = (int)gender;
        row.Outfit = outfit;
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task SetDeletionAsync(
        Guid characterId,
        DateTimeOffset? requestedAt,
        DateTimeOffset? scheduledFor,
        CancellationToken cancellationToken = default)
    {
        var row = await _context.Characters.FirstOrDefaultAsync(x => x.Id == characterId, cancellationToken);

        if (row is null)
        {
            return;
        }

        row.DeletionRequestedAtUnixMilliseconds = requestedAt?.ToUnixTimeMilliseconds();
        row.DeletionScheduledForUnixMilliseconds = scheduledFor?.ToUnixTimeMilliseconds();
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task<int> FinalizeDueDeletionsAsync(
        Guid accountId,
        DateTimeOffset now,
        CancellationToken cancellationToken = default)
    {
        var nowMs = now.ToUnixTimeMilliseconds();

        var due = await _context.Characters
            .Where(x => x.AccountId == accountId
                && x.DeletedAtUnixMilliseconds == null
                && x.DeletionScheduledForUnixMilliseconds != null
                && x.DeletionScheduledForUnixMilliseconds <= nowMs)
            .ToListAsync(cancellationToken);

        foreach (var row in due)
        {
            row.DeletedAtUnixMilliseconds = nowMs;
        }

        if (due.Count > 0)
        {
            await _context.SaveChangesAsync(cancellationToken);
        }

        return due.Count;
    }

    public async Task<bool> NameInUseAsync(string name, CancellationToken cancellationToken = default)
    {
        var normalized = name.ToLower();

        return await _context.Characters.AnyAsync(
            x => x.DeletedAtUnixMilliseconds == null && x.Name.ToLower() == normalized,
            cancellationToken);
    }

    private static DateTimeOffset? FromUnix(long? milliseconds) =>
        milliseconds is null ? null : DateTimeOffset.FromUnixTimeMilliseconds(milliseconds.Value);
}
