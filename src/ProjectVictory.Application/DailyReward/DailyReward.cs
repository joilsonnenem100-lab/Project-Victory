using System.Collections.Concurrent;
using ProjectVictory.Application.Supply;
using ProjectVictory.Domain.Character;

namespace ProjectVictory.Application.DailyReward;

// PV-DAILY-010 (PO-16, PO-60; R-068): recompensa diária POR CONTA — lista de itens (e ouro) entregue na
// primeira entrada no mundo depois do horário de reset, no máximo uma vez por ciclo; decidida no SERVIDOR.
// *** PROVISÓRIO *** — itens, quantidades, ouro e horário vêm da configuração "DailyReward"
// (appsettings.json). Sem itens e sem ouro = desligada.
public sealed class DailyRewardSettings
{
    public const string ResetTimeUtcKey = "DailyReward:ResetTimeUtc";
    public const string GoldKey = "DailyReward:Gold";
    public const string ItemsKey = "DailyReward:Items";

    // Item por Id (só poções do PotionCatalog nesta fase, como o kit inicial).
    public IReadOnlyDictionary<Guid, int> Items { get; }

    public long Gold { get; }

    // Horário do reset diário em UTC (hora do dia).
    public TimeSpan ResetTimeUtc { get; }

    public bool Enabled => Gold > 0 || Items.Values.Any(quantity => quantity > 0);

    public static DailyRewardSettings Disabled { get; } = new(new Dictionary<Guid, int>(), 0, TimeSpan.Zero);

    public DailyRewardSettings(IReadOnlyDictionary<Guid, int> items, long gold, TimeSpan resetTimeUtc)
    {
        ArgumentNullException.ThrowIfNull(items);

        if (gold < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(gold), "Daily reward gold cannot be negative.");
        }

        if (resetTimeUtc < TimeSpan.Zero || resetTimeUtc >= TimeSpan.FromDays(1))
        {
            throw new ArgumentOutOfRangeException(nameof(resetTimeUtc), "Daily reset time must be within a day (00:00 to 23:59).");
        }

        foreach (var (itemId, quantity) in items)
        {
            if (PotionCatalog.Find(itemId) is null)
            {
                throw new ArgumentException($"Daily reward item {itemId} is not a potion.", nameof(items));
            }

            if (quantity < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(items), "Daily reward quantities cannot be negative.");
            }
        }

        Items = new Dictionary<Guid, int>(items);
        Gold = gold;
        ResetTimeUtc = resetTimeUtc;
    }

    // Itens por nome (Key do PotionCatalog), como o kit inicial ("DailyReward:Items:<Key>").
    public static IReadOnlyDictionary<Guid, int> ParseItems(IEnumerable<KeyValuePair<string, int>> entries)
    {
        ArgumentNullException.ThrowIfNull(entries);

        var items = new Dictionary<Guid, int>();

        foreach (var (key, quantity) in entries)
        {
            var potion = PotionCatalog.FindByKey(key)
                ?? throw new InvalidOperationException(
                    $"{ItemsKey}:{key} não é uma poção conhecida ({string.Join(", ", PotionCatalog.All.Select(p => p.Key))}).");

            if (quantity < 0)
            {
                throw new InvalidOperationException($"{ItemsKey}:{key} não pode ser negativo.");
            }

            items[potion.ItemId] = items.GetValueOrDefault(potion.ItemId) + quantity;
        }

        return items;
    }

    // Início do ciclo diário que contém "now": o último instante <= now com hora do dia = ResetTimeUtc.
    public DateTimeOffset CycleStartFor(DateTimeOffset now)
    {
        var utc = now.ToUniversalTime();
        var todayReset = new DateTimeOffset(utc.Date, TimeSpan.Zero) + ResetTimeUtc;
        return utc >= todayReset ? todayReset : todayReset.AddDays(-1);
    }
}

// Recebimento por conta e ciclo (persistência do "já recebeu neste ciclo").
public sealed record DailyRewardClaim(Guid AccountId, DateTimeOffset CycleStart, DateTimeOffset ClaimedAt, Guid CharacterId);

public interface IDailyRewardClaimStore
{
    Task<DailyRewardClaim?> GetLastAsync(Guid accountId, CancellationToken cancellationToken = default);

    // Grava o recebimento do ciclo; false se a conta já recebeu neste ciclo (chave conta + ciclo).
    Task<bool> TryAddAsync(DailyRewardClaim claim, CancellationToken cancellationToken = default);
}

// Uma concessão por conta de cada vez (duas entradas simultâneas não entregam duas vezes).
public sealed class DailyRewardGate
{
    private readonly ConcurrentDictionary<Guid, SemaphoreSlim> _gates = new();

    public async Task<IDisposable> AcquireAsync(Guid accountId, CancellationToken cancellationToken = default)
    {
        var semaphore = _gates.GetOrAdd(accountId, _ => new SemaphoreSlim(1, 1));
        await semaphore.WaitAsync(cancellationToken);
        return new Releaser(semaphore);
    }

    private sealed class Releaser(SemaphoreSlim semaphore) : IDisposable
    {
        private int _disposed;

        public void Dispose()
        {
            if (Interlocked.Exchange(ref _disposed, 1) == 0)
            {
                semaphore.Release();
            }
        }
    }
}

public sealed record DailyRewardItemView(Guid ItemDefinitionId, string Name, int Quantity);

public sealed record DailyRewardGrant(IReadOnlyList<DailyRewardItemView> Items, long Gold, DateTimeOffset CycleStart);

// Concede a recompensa ao personagem que entra, se a CONTA ainda não recebeu no ciclo atual. A conta e o
// personagem vêm do EnterGameHandler (identidade do token, PV-SEC-001). Os itens e o ouro entram no
// Inventory pelos métodos do próprio agregado Character (mesmo caminho do kit inicial e do Reward
// Pipeline: Inventory.AddItem/AddGold + ICharacterRepository.UpdateAsync). Sem Experience (ADR-024 intacto).
public sealed class GrantDailyRewardHandler(
    DailyRewardSettings settings,
    IDailyRewardClaimStore claims,
    ICharacterRepository characters,
    DailyRewardGate gate,
    TimeProvider clock)
{
    public async Task<DailyRewardGrant?> TryGrantAsync(
        Guid accountId,
        ProjectVictory.Domain.Entities.Character character,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(character);

        if (!settings.Enabled)
        {
            return null;
        }

        using var _ = await gate.AcquireAsync(accountId, cancellationToken);

        var now = clock.GetUtcNow();
        var cycleStart = settings.CycleStartFor(now);
        var last = await claims.GetLastAsync(accountId, cancellationToken);

        if (last is not null && last.CycleStart >= cycleStart)
        {
            return null;
        }

        // Primeiro o registro (a chave conta + ciclo impede a segunda entrega); depois o inventário.
        // Falha entre os dois = recompensa perdida no ciclo, nunca duplicada (registrado no contrato).
        if (!await claims.TryAddAsync(new DailyRewardClaim(accountId, cycleStart, now, character.Id), cancellationToken))
        {
            return null;
        }

        foreach (var (itemId, quantity) in settings.Items)
        {
            for (var i = 0; i < quantity; i++)
            {
                character.Inventory.AddItem(new global::ItemInstance(Guid.NewGuid(), itemId));
            }
        }

        if (settings.Gold > 0)
        {
            character.Inventory.AddGold(settings.Gold);
        }

        await characters.UpdateAsync(character, cancellationToken);

        return new DailyRewardGrant(
            settings.Items
                .Where(item => item.Value > 0)
                .Select(item => new DailyRewardItemView(item.Key, PotionCatalog.Find(item.Key)!.Item.Name, item.Value))
                .ToList(),
            settings.Gold,
            cycleStart);
    }
}
