using System.Collections.Concurrent;
using ProjectVictory.Application.Events;
using ProjectVictory.Application.Reward;
using ProjectVictory.Application.Session;
using ProjectVictory.Domain.Combat;
using ProjectVictory.Domain.Events;
using ProjectVictory.Domain.Hunt;
using ProjectVictory.Domain.Items;
using ProjectVictory.Domain.Loot;

namespace ProjectVictory.Application.HuntAnalyzer;

// PV-HUD-ANALYZER-010 (DEC-20261106-01/08, PO-47): Hunt Analyzer calculado no servidor — tempo real
// durante a Hunt, resumo ao fim e histórico das últimas Hunts. O Analyzer só LÊ fatos de Combat,
// Reward e Supply (Aggregate Ownership); a regra de cálculo está no Domain (HuntAnalyzerCalculator).

// *** PROVISÓRIO *** — quantas Hunts o histórico guarda por personagem (PO-47 não fixa o número).
// Configurável por "HuntAnalyzer:HistorySize" (Program.cs).
public sealed class HuntAnalyzerSettings
{
    public const string HistorySizeKey = "HuntAnalyzer:HistorySize";

    public const int DefaultHistorySize = 10;

    public static HuntAnalyzerSettings Default { get; } = new();

    public int HistorySize { get; }

    public HuntAnalyzerSettings(int historySize = DefaultHistorySize)
    {
        if (historySize < 1)
        {
            throw new ArgumentOutOfRangeException(nameof(historySize), "History size must be at least 1.");
        }

        HistorySize = historySize;
    }
}

// Retrato da apuração de uma Hunt (quantidades por definição de item).
public sealed record HuntAnalyzerTallySnapshot(
    Guid HuntId,
    Guid CharacterId,
    IReadOnlyDictionary<Guid, int> SuppliesUsed,
    IReadOnlyDictionary<Guid, int> Loot,
    long Gold)
{
    public static HuntAnalyzerTallySnapshot Empty(Guid huntId, Guid characterId) =>
        new(huntId, characterId, new Dictionary<Guid, int>(), new Dictionary<Guid, int>(), 0);
}

// PV-HUD-ANALYZER-010: apuração da Hunt em andamento (Temporário, em memória, por HuntId).
// *** PROVISÓRIO *** — reinício do servidor perde a apuração da Hunt em curso (a Hunt também termina,
// ADR-057 / DEC-20261106-47); o resumo dela fica só com XP e tempo.
public interface IHuntAnalyzerTally
{
    // Abre a apuração (se preciso) e soma as poções usadas na rodada.
    void RecordSupplies(Guid huntId, Guid characterId, int healthPotionsUsed, int manaPotionsUsed);

    // PV-CONTENT-012: poções usadas por nível (Id do item → quantidade); o débito usa o preço de cada nível.
    void RecordSupplyItems(Guid huntId, Guid characterId, IReadOnlyDictionary<Guid, int> used);

    // Soma loot e ouro SÓ de Hunts com apuração aberta (o loot fora de Hunt não é contado).
    void RecordLoot(Guid huntId, LootResolutionResult loot);

    HuntAnalyzerTallySnapshot? Get(Guid huntId);

    HuntAnalyzerTallySnapshot? Remove(Guid huntId);
}

public sealed class InMemoryHuntAnalyzerTally : IHuntAnalyzerTally
{
    private sealed class Entry
    {
        public required Guid CharacterId { get; init; }

        public Dictionary<Guid, int> Supplies { get; } = new();

        public Dictionary<Guid, int> Loot { get; } = new();

        public long Gold { get; set; }
    }

    private readonly ConcurrentDictionary<Guid, Entry> _entries = new();

    public void RecordSupplies(Guid huntId, Guid characterId, int healthPotionsUsed, int manaPotionsUsed)
    {
        var entry = _entries.GetOrAdd(huntId, _ => new Entry { CharacterId = characterId });

        lock (entry)
        {
            Add(entry.Supplies, ProjectVictory.Application.Supply.SupplyCatalog.HealthPotionId, healthPotionsUsed);
            Add(entry.Supplies, ProjectVictory.Application.Supply.SupplyCatalog.ManaPotionId, manaPotionsUsed);
        }
    }

    // PV-CONTENT-012: abre a apuração (como RecordSupplies) e soma cada nível de poção.
    public void RecordSupplyItems(Guid huntId, Guid characterId, IReadOnlyDictionary<Guid, int> used)
    {
        ArgumentNullException.ThrowIfNull(used);

        var entry = _entries.GetOrAdd(huntId, _ => new Entry { CharacterId = characterId });

        lock (entry)
        {
            foreach (var (itemId, quantity) in used)
            {
                Add(entry.Supplies, itemId, quantity);
            }
        }
    }

    public void RecordLoot(Guid huntId, LootResolutionResult loot)
    {
        ArgumentNullException.ThrowIfNull(loot);

        if (!_entries.TryGetValue(huntId, out var entry))
        {
            return;
        }

        lock (entry)
        {
            foreach (var item in loot.Items)
            {
                Add(entry.Loot, item.ItemDefinitionId, 1);
            }

            entry.Gold = checked(entry.Gold + loot.Gold);
        }
    }

    public HuntAnalyzerTallySnapshot? Get(Guid huntId) =>
        _entries.TryGetValue(huntId, out var entry) ? Snapshot(huntId, entry) : null;

    public HuntAnalyzerTallySnapshot? Remove(Guid huntId) =>
        _entries.TryRemove(huntId, out var entry) ? Snapshot(huntId, entry) : null;

    private static HuntAnalyzerTallySnapshot Snapshot(Guid huntId, Entry entry)
    {
        lock (entry)
        {
            return new HuntAnalyzerTallySnapshot(
                huntId,
                entry.CharacterId,
                new Dictionary<Guid, int>(entry.Supplies),
                new Dictionary<Guid, int>(entry.Loot),
                entry.Gold);
        }
    }

    private static void Add(Dictionary<Guid, int> counts, Guid itemDefinitionId, int quantity)
    {
        if (quantity <= 0)
        {
            return;
        }

        counts[itemDefinitionId] = checked(counts.GetValueOrDefault(itemDefinitionId) + quantity);
    }
}

// PV-HUD-ANALYZER-010: liga o Reward (que não conhece o Analyzer) à apuração — só leitura.
public sealed class HuntAnalyzerLootObserver : IRewardDeliveryObserver
{
    private readonly IHuntAnalyzerTally _tally;

    public HuntAnalyzerLootObserver(IHuntAnalyzerTally tally)
    {
        _tally = tally;
    }

    public void OnLootDelivered(CombatResult combatResult, LootResolutionResult loot)
    {
        ArgumentNullException.ThrowIfNull(combatResult);
        _tally.RecordLoot(combatResult.HuntId, loot);
    }
}

// PV-HUD-ANALYZER-010: valora a apuração com os preços NPC da definição do item e chama a regra do Domain.
// *** PROVISÓRIO *** — preço lido no momento do cálculo (não congelado na hora do loot); item sem
// definição vale 0.
public sealed class HuntAnalyzerPricing
{
    private readonly IItemDefinitionRepository _items;

    public HuntAnalyzerPricing(IItemDefinitionRepository items)
    {
        _items = items;
    }

    public async Task<HuntAnalyzerReport> CalculateAsync(
        long experienceGained,
        long elapsedSeconds,
        HuntAnalyzerTallySnapshot tally,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(tally);

        var definitions = new Dictionary<Guid, ItemDefinition?>();

        async Task<ItemDefinition?> Definition(Guid id)
        {
            if (!definitions.TryGetValue(id, out var definition))
            {
                definition = await _items.GetByIdAsync(id, cancellationToken);
                definitions[id] = definition;
            }

            return definition;
        }

        var supplies = new List<HuntAnalyzerLine>();

        foreach (var (id, quantity) in tally.SuppliesUsed)
        {
            supplies.Add(new HuntAnalyzerLine(id, quantity, (await Definition(id))?.NpcBuyPrice ?? 0));
        }

        var loot = new List<HuntAnalyzerLine>();

        foreach (var (id, quantity) in tally.Loot)
        {
            loot.Add(new HuntAnalyzerLine(id, quantity, (await Definition(id))?.NpcSellPrice ?? 0));
        }

        return HuntAnalyzerCalculator.Calculate(experienceGained, elapsedSeconds, supplies, loot, tally.Gold);
    }
}

// PV-HUD-ANALYZER-010: resumo persistido de uma Hunt encerrada.
public sealed record HuntAnalyzerSummary(
    Guid HuntId,
    Guid CharacterId,
    string EndReason,
    DateTimeOffset EndedAt,
    HuntAnalyzerReport Report);

// PV-HUD-ANALYZER-010: histórico (Permanente; a Persistence implementa).
public interface IHuntAnalyzerHistoryStore
{
    // Grava o resumo e mantém só os `keep` mais recentes do personagem.
    Task AddAsync(HuntAnalyzerSummary summary, int keep, CancellationToken cancellationToken = default);

    // Mais recente primeiro.
    Task<IReadOnlyList<HuntAnalyzerSummary>> GetRecentAsync(Guid characterId, int count, CancellationToken cancellationToken = default);
}

// DTOs da API (o cliente só exibe, ADR-002).
public sealed record HuntAnalyzerDto(
    long ExperienceGained,
    long ElapsedSeconds,
    long ExperiencePerHour,
    long Debit,
    long LootValue,
    long Gold,
    long Profit,
    long NetProfit,
    int SuppliesUsed,
    int ItemsLooted)
{
    public static HuntAnalyzerDto From(HuntAnalyzerReport report) =>
        new(report.ExperienceGained, report.ElapsedSeconds, report.ExperiencePerHour, report.Debit,
            report.LootValue, report.Gold, report.Profit, report.NetProfit, report.SuppliesUsed, report.ItemsLooted);
}

public sealed record HuntAnalyzerSummaryDto(
    Guid HuntId,
    string EndReason,
    DateTimeOffset EndedAt,
    HuntAnalyzerDto Analyzer)
{
    public static HuntAnalyzerSummaryDto From(HuntAnalyzerSummary summary) =>
        new(summary.HuntId, summary.EndReason, summary.EndedAt, HuntAnalyzerDto.From(summary.Report));
}

public sealed record HuntAnalyzerLiveResult(
    bool HasActiveHunt,
    Guid? HuntId,
    HuntAnalyzerDto? Analyzer,
    HuntAnalyzerSummaryDto? LastHunt);

public sealed record GetHuntAnalyzerQuery(Guid CharacterId);

public sealed record GetHuntAnalyzerHistoryQuery(Guid CharacterId);

// PV-HUD-ANALYZER-010: Analyzer em tempo real (Hunt em andamento) ou, sem Hunt, o resumo da última.
// Posse pelo token (PV-SEC-001): personagem de outra conta ou inexistente → OwnershipViolation (403).
public sealed class GetHuntAnalyzerHandler
{
    private readonly PlayerOwnershipGuard _ownership;
    private readonly IHuntRepository _hunts;
    private readonly IHuntAnalyzerTally _tally;
    private readonly HuntAnalyzerPricing _pricing;
    private readonly IHuntAnalyzerHistoryStore _history;
    private readonly TimeProvider _time;

    public GetHuntAnalyzerHandler(
        PlayerOwnershipGuard ownership,
        IHuntRepository hunts,
        IHuntAnalyzerTally tally,
        HuntAnalyzerPricing pricing,
        IHuntAnalyzerHistoryStore history,
        TimeProvider time)
    {
        _ownership = ownership;
        _hunts = hunts;
        _tally = tally;
        _pricing = pricing;
        _history = history;
        _time = time;
    }

    public async Task<HuntAnalyzerLiveResult> Handle(GetHuntAnalyzerQuery query, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(query);

        await _ownership.EnsureCharacterAsync(query.CharacterId, cancellationToken);

        var running = await _hunts.GetRunningByCharacterIdAsync(query.CharacterId, cancellationToken);

        if (running is null)
        {
            var last = (await _history.GetRecentAsync(query.CharacterId, 1, cancellationToken)).FirstOrDefault();
            return new HuntAnalyzerLiveResult(false, null, null, last is null ? null : HuntAnalyzerSummaryDto.From(last));
        }

        // *** PROVISÓRIO *** — tempo online = duração da Hunt (ADR-057: a Hunt só existe online).
        var elapsedSeconds = (long)Math.Floor(running.ElapsedAt(_time.GetUtcNow()).TotalSeconds);
        var tally = _tally.Get(running.Id) ?? HuntAnalyzerTallySnapshot.Empty(running.Id, running.CharacterId);
        var report = await _pricing.CalculateAsync(running.ExperienceGained, elapsedSeconds, tally, cancellationToken);

        return new HuntAnalyzerLiveResult(true, running.Id, HuntAnalyzerDto.From(report), null);
    }
}

// PV-HUD-ANALYZER-010 (PO-47): histórico das últimas Hunts do personagem (mais recente primeiro).
public sealed class GetHuntAnalyzerHistoryHandler
{
    private readonly PlayerOwnershipGuard _ownership;
    private readonly IHuntAnalyzerHistoryStore _history;
    private readonly HuntAnalyzerSettings _settings;

    public GetHuntAnalyzerHistoryHandler(
        PlayerOwnershipGuard ownership,
        IHuntAnalyzerHistoryStore history,
        HuntAnalyzerSettings settings)
    {
        _ownership = ownership;
        _history = history;
        _settings = settings;
    }

    public async Task<IReadOnlyList<HuntAnalyzerSummaryDto>> Handle(GetHuntAnalyzerHistoryQuery query, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(query);

        await _ownership.EnsureCharacterAsync(query.CharacterId, cancellationToken);

        return (await _history.GetRecentAsync(query.CharacterId, _settings.HistorySize, cancellationToken))
            .Select(HuntAnalyzerSummaryDto.From)
            .ToList();
    }
}

// PV-HUD-ANALYZER-010: assinante do HuntEndedEvent (ADR-012: publicado pelo pipeline único; ADR-058:
// notificação isolada — falha aqui não desfaz o fim da Hunt). Calcula o resumo, grava no histórico
// (últimas N) e descarta a apuração em memória. Não é assinante obrigatório (sem ele o jogo segue).
public sealed class HuntAnalyzerOnHuntEnded : IAsyncEventHandler<HuntEndedEvent>
{
    private readonly IHuntAnalyzerTally _tally;
    private readonly HuntAnalyzerPricing _pricing;
    private readonly IHuntAnalyzerHistoryStore _history;
    private readonly HuntAnalyzerSettings _settings;

    public HuntAnalyzerOnHuntEnded(
        IHuntAnalyzerTally tally,
        HuntAnalyzerPricing pricing,
        IHuntAnalyzerHistoryStore history,
        HuntAnalyzerSettings settings)
    {
        _tally = tally;
        _pricing = pricing;
        _history = history;
        _settings = settings;
    }

    public async Task HandleAsync(HuntEndedEvent domainEvent, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(domainEvent);

        var tally = _tally.Remove(domainEvent.HuntId)
            ?? HuntAnalyzerTallySnapshot.Empty(domainEvent.HuntId, domainEvent.CharacterId);

        var report = await _pricing.CalculateAsync(
            domainEvent.ExperienceGained,
            Math.Max(0, domainEvent.ElapsedSeconds),
            tally,
            cancellationToken);

        await _history.AddAsync(
            new HuntAnalyzerSummary(domainEvent.HuntId, domainEvent.CharacterId, domainEvent.Reason, domainEvent.OccurredAt, report),
            _settings.HistorySize,
            cancellationToken);
    }
}
