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
public interface IPendingSupplyConsumption
{
    void Add(Guid characterId, int healthPotions, int manaPotions);

    (int HealthPotions, int ManaPotions) Peek(Guid characterId);

    (int HealthPotions, int ManaPotions) Take(Guid characterId);
}

public sealed class PendingSupplyConsumption : IPendingSupplyConsumption
{
    private readonly ConcurrentDictionary<Guid, (int HealthPotions, int ManaPotions)> _pending = new();

    public void Add(Guid characterId, int healthPotions, int manaPotions)
    {
        if (healthPotions < 0 || manaPotions < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(healthPotions), "Consumption cannot be negative.");
        }

        if (healthPotions + manaPotions == 0)
        {
            return;
        }

        _pending.AddOrUpdate(
            characterId,
            (healthPotions, manaPotions),
            (_, current) => (current.HealthPotions + healthPotions, current.ManaPotions + manaPotions));
    }

    public (int HealthPotions, int ManaPotions) Peek(Guid characterId) =>
        _pending.TryGetValue(characterId, out var value) ? value : (0, 0);

    public (int HealthPotions, int ManaPotions) Take(Guid characterId) =>
        _pending.TryRemove(characterId, out var value) ? value : (0, 0);
}

// PV-PERSIST-011: registro (em memória) dos checkpoints feitos — observabilidade e testes.
public interface ICheckpointLog
{
    void Record(Guid characterId, CheckpointReason reason);

    IReadOnlyList<CheckpointReason> For(Guid characterId);
}

public sealed class CheckpointLog : ICheckpointLog
{
    private readonly ConcurrentDictionary<Guid, ConcurrentQueue<CheckpointReason>> _entries = new();

    public void Record(Guid characterId, CheckpointReason reason) =>
        _entries.GetOrAdd(characterId, _ => new ConcurrentQueue<CheckpointReason>()).Enqueue(reason);

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
        var (healthPotions, manaPotions) = _pendingSupplies.Take(characterId);

        if (healthPotions + manaPotions > 0)
        {
            var character =
                await _characterRepository.GetByIdAsync(characterId, cancellationToken)
                ?? throw new InvalidOperationException("Character not found.");

            RemovePotions(character, SupplyCatalog.HealthPotionId, healthPotions);
            RemovePotions(character, SupplyCatalog.ManaPotionId, manaPotions);

            await _characterRepository.UpdateAsync(character, cancellationToken);
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
