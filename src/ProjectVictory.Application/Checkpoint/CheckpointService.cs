using System.Collections.Concurrent;
using ProjectVictory.Application.Supply;
using ProjectVictory.Application.VerticalSlice;
using ProjectVictory.Domain.Character;

namespace ProjectVictory.Application.Checkpoint;

// PV-PERSIST-011 (Handbook §19, "Checkpoint Strategy"): salvar em momentos relevantes — início,
// conclusão, recompensa e logout — e EVITAR salvar cada ação pequena.
public enum CheckpointReason
{
    HuntStart,
    Reward,
    HuntConclusion,
    Logout
}

// PV-PERSIST-011: poções usadas nas rodadas e AINDA NÃO gravadas (estado Temporário, em memória).
// Uma rodada de combate é "ação pequena": o consumo fica aqui até o próximo checkpoint.
// PV-CONTENT-012 (PO-15): o pendente é guardado POR NÍVEL de poção (Id do item). Os métodos antigos
// (vida/Mana) continuam: Add/Subtract sem nível = nível pequeno; Peek/Take devolvem o total do tipo.
public interface IPendingSupplyConsumption
{
    void Add(Guid characterId, int healthPotions, int manaPotions);

    (int HealthPotions, int ManaPotions) Peek(Guid characterId);

    (int HealthPotions, int ManaPotions) Take(Guid characterId);

    // Revisão do PV-PERSIST-011: retira só o que foi gravado (o que entrar depois continua pendente).
    void Subtract(Guid characterId, int healthPotions, int manaPotions);

    // PV-CONTENT-012: por Id do item.
    void AddItems(Guid characterId, IReadOnlyDictionary<Guid, int> used);

    IReadOnlyDictionary<Guid, int> PeekItems(Guid characterId);

    void SubtractItems(Guid characterId, IReadOnlyDictionary<Guid, int> saved);
}

public sealed class PendingSupplyConsumption : IPendingSupplyConsumption
{
    private readonly Dictionary<Guid, Dictionary<Guid, int>> _pending = new();
    private readonly object _gate = new();

    public void Add(Guid characterId, int healthPotions, int manaPotions)
    {
        if (healthPotions < 0 || manaPotions < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(healthPotions), "Consumption cannot be negative.");
        }

        AddItems(characterId, new Dictionary<Guid, int>
        {
            [SupplyCatalog.HealthPotionId] = healthPotions,
            [SupplyCatalog.ManaPotionId] = manaPotions
        });
    }

    public (int HealthPotions, int ManaPotions) Peek(Guid characterId) => ByKind(PeekItems(characterId));

    public (int HealthPotions, int ManaPotions) Take(Guid characterId)
    {
        lock (_gate)
        {
            if (!_pending.Remove(characterId, out var items))
            {
                return (0, 0);
            }

            return ByKind(items);
        }
    }

    public void Subtract(Guid characterId, int healthPotions, int manaPotions)
    {
        if (healthPotions < 0 || manaPotions < 0)
        {
            return;
        }

        // Compatibilidade: retira do total do tipo, começando pelo nível pequeno.
        lock (_gate)
        {
            if (!_pending.TryGetValue(characterId, out var items))
            {
                return;
            }

            SubtractKind(items, PotionKind.Health, healthPotions);
            SubtractKind(items, PotionKind.Mana, manaPotions);
            Prune(characterId, items);
        }
    }

    public void AddItems(Guid characterId, IReadOnlyDictionary<Guid, int> used)
    {
        ArgumentNullException.ThrowIfNull(used);

        if (used.Values.Any(quantity => quantity < 0))
        {
            throw new ArgumentOutOfRangeException(nameof(used), "Consumption cannot be negative.");
        }

        if (used.Values.Sum() == 0)
        {
            return;
        }

        lock (_gate)
        {
            if (!_pending.TryGetValue(characterId, out var items))
            {
                items = new Dictionary<Guid, int>();
                _pending[characterId] = items;
            }

            foreach (var (itemId, quantity) in used)
            {
                if (quantity > 0)
                {
                    items[itemId] = items.GetValueOrDefault(itemId) + quantity;
                }
            }
        }
    }

    public IReadOnlyDictionary<Guid, int> PeekItems(Guid characterId)
    {
        lock (_gate)
        {
            return _pending.TryGetValue(characterId, out var items)
                ? new Dictionary<Guid, int>(items)
                : new Dictionary<Guid, int>();
        }
    }

    public void SubtractItems(Guid characterId, IReadOnlyDictionary<Guid, int> saved)
    {
        ArgumentNullException.ThrowIfNull(saved);

        lock (_gate)
        {
            if (!_pending.TryGetValue(characterId, out var items))
            {
                return;
            }

            foreach (var (itemId, quantity) in saved)
            {
                if (quantity > 0 && items.TryGetValue(itemId, out var current))
                {
                    items[itemId] = Math.Max(0, current - quantity);
                }
            }

            Prune(characterId, items);
        }
    }

    private void Prune(Guid characterId, Dictionary<Guid, int> items)
    {
        foreach (var empty in items.Where(x => x.Value <= 0).Select(x => x.Key).ToList())
        {
            items.Remove(empty);
        }

        if (items.Count == 0)
        {
            _pending.Remove(characterId);
        }
    }

    private static void SubtractKind(Dictionary<Guid, int> items, PotionKind kind, int quantity)
    {
        foreach (var potion in PotionCatalog.All.Where(p => p.Kind == kind).OrderBy(p => p.Size))
        {
            if (quantity <= 0)
            {
                return;
            }

            if (items.TryGetValue(potion.ItemId, out var current) && current > 0)
            {
                var taken = Math.Min(current, quantity);
                items[potion.ItemId] = current - taken;
                quantity -= taken;
            }
        }
    }

    private static (int HealthPotions, int ManaPotions) ByKind(IReadOnlyDictionary<Guid, int> items) =>
        (items.Where(x => PotionCatalog.Find(x.Key)?.Kind == PotionKind.Health).Sum(x => x.Value),
         items.Where(x => PotionCatalog.Find(x.Key)?.Kind == PotionKind.Mana).Sum(x => x.Value));
}

// PV-PERSIST-011: registro (em memória) dos checkpoints feitos — observabilidade e testes.
public interface ICheckpointLog
{
    void Record(Guid characterId, CheckpointReason reason);

    IReadOnlyList<CheckpointReason> For(Guid characterId);
}

// Revisão do PV-PERSIST-011 (ressalva 3): guarda só os últimos `MaximumPerCharacter` registros por
// personagem (memória limitada). O número de personagens continua sem limite (mesmo padrão dos
// demais stores em memória; R-044).
public sealed class CheckpointLog : ICheckpointLog
{
    public const int MaximumPerCharacter = 32;

    private readonly ConcurrentDictionary<Guid, ConcurrentQueue<CheckpointReason>> _entries = new();

    public void Record(Guid characterId, CheckpointReason reason)
    {
        var queue = _entries.GetOrAdd(characterId, _ => new ConcurrentQueue<CheckpointReason>());
        queue.Enqueue(reason);

        while (queue.Count > MaximumPerCharacter && queue.TryDequeue(out _))
        {
        }
    }

    public IReadOnlyList<CheckpointReason> For(Guid characterId) =>
        _entries.TryGetValue(characterId, out var queue) ? queue.ToList() : [];
}

public interface ICheckpointService
{
    Task SaveAsync(
        Guid characterId,
        Guid runtimeId,
        CheckpointReason reason,
        CancellationToken cancellationToken = default);
}

// PV-PERSIST-011: um checkpoint grava, de uma vez:
// 1. o consumo de poções pendente → Inventory do personagem (Permanente);
// 2. a cópia do Runtime (vida/mana atuais) → RuntimeRepository (Semi-permanente).
// A Experience/loot da recompensa continuam sendo gravados pelo Reward Pipeline (ADR-024).
public sealed class CheckpointService : ICheckpointService
{
    private readonly ICharacterRepository _characterRepository;
    private readonly IRuntimeRepository _runtimeRepository;
    private readonly IRuntimeExecutionStateStore _runtimeExecutionStateStore;
    private readonly IPendingSupplyConsumption _pendingSupplies;
    private readonly ICheckpointLog _log;

    public CheckpointService(
        ICharacterRepository characterRepository,
        IRuntimeRepository runtimeRepository,
        IRuntimeExecutionStateStore runtimeExecutionStateStore,
        IPendingSupplyConsumption pendingSupplies,
        ICheckpointLog log)
    {
        _characterRepository = characterRepository;
        _runtimeRepository = runtimeRepository;
        _runtimeExecutionStateStore = runtimeExecutionStateStore;
        _pendingSupplies = pendingSupplies;
        _log = log;
    }

    public async Task SaveAsync(
        Guid characterId,
        Guid runtimeId,
        CheckpointReason reason,
        CancellationToken cancellationToken = default)
    {
        // Revisão do PV-PERSIST-011 (ressalva 2): lê o pendente, GRAVA e só então o retira — se a
        // gravação falhar, o débito continua pendente (não se perde).
        // PV-CONTENT-012: por nível de poção (Id do item).
        var pending = _pendingSupplies.PeekItems(characterId);

        if (pending.Values.Sum() > 0)
        {
            var character =
                await _characterRepository.GetByIdAsync(characterId, cancellationToken)
                ?? throw new InvalidOperationException("Character not found.");

            foreach (var (itemDefinitionId, count) in pending)
            {
                RemovePotions(character, itemDefinitionId, count);
            }

            await _characterRepository.UpdateAsync(character, cancellationToken);

            _pendingSupplies.SubtractItems(characterId, pending);
        }

        if (runtimeId != Guid.Empty
            && _runtimeExecutionStateStore.Contains(runtimeId)
            && _runtimeExecutionStateStore.Get(runtimeId).CharacterState is { } state)
        {
            await _runtimeRepository.SaveCheckpointAsync(
                runtimeId,
                state.CurrentHealth,
                state.CurrentMana,
                cancellationToken);
        }

        _log.Record(characterId, reason);
    }

    private static void RemovePotions(
        ProjectVictory.Domain.Entities.Character character,
        Guid itemDefinitionId,
        int count)
    {
        var instances = character.Inventory.Items
            .Where(item => item.ItemDefinitionId == itemDefinitionId)
            .Take(count)
            .Select(item => item.Id)
            .ToList();

        foreach (var instanceId in instances)
        {
            character.Inventory.RemoveItem(instanceId);
        }
    }
}
