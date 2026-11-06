using System;

namespace ProjectVictory.Domain.Spawn;

// PV-SPAWN-010 (ADR-047; developer-guide/domain/spawn.md): estados de uma Spawn Instance.
public enum SpawnState
{
    Created,
    Spawning,
    Active,
    Despawning,
    Despawned
}

// PV-SPAWN-010 (ADR-009/047): Spawn Instance EFÊMERA — nunca é persistida e não guarda regra de
// domínio (combate, IA, loot e XP ficam fora). Percorre uma máquina de estados explícita:
// Created → Spawning → Active → Despawning → Despawned (transição fora da ordem = erro).
public sealed class SpawnInstance
{
    public SpawnInstance(Guid spawnInstanceId, Guid spawnDefinitionId)
    {
        if (spawnInstanceId == Guid.Empty)
        {
            throw new ArgumentException("SpawnInstanceId is required.", nameof(spawnInstanceId));
        }

        if (spawnDefinitionId == Guid.Empty)
        {
            throw new ArgumentException("SpawnDefinitionId is required.", nameof(spawnDefinitionId));
        }

        SpawnInstanceId = spawnInstanceId;
        SpawnDefinitionId = spawnDefinitionId;
        CurrentState = SpawnState.Created;
    }

    public Guid SpawnInstanceId { get; }

    // A definição da entidade criada (hoje: o MonsterId da definição de dados do monstro).
    public Guid SpawnDefinitionId { get; }

    public SpawnState CurrentState { get; private set; }

    public DateTime? SpawnedAt { get; private set; }

    public DateTime? DespawnedAt { get; private set; }

    public void BeginSpawning() => Move(SpawnState.Created, SpawnState.Spawning);

    public void Activate(DateTime now)
    {
        Move(SpawnState.Spawning, SpawnState.Active);
        SpawnedAt = now;
    }

    public void BeginDespawning() => Move(SpawnState.Active, SpawnState.Despawning);

    public void Despawn(DateTime now)
    {
        Move(SpawnState.Despawning, SpawnState.Despawned);
        DespawnedAt = now;
    }

    private void Move(SpawnState expected, SpawnState next)
    {
        if (CurrentState != expected)
        {
            throw new InvalidOperationException(
                $"Invalid spawn transition: {CurrentState} → {next} (expected {expected}).");
        }

        CurrentState = next;
    }
}
