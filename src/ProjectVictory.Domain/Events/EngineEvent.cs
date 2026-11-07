using System;
using ProjectVictory.Domain.Combat;

namespace ProjectVictory.Domain.Events;

// PV-EVENT-010 (ADR-049): todo evento do Event Bus deriva de EngineEvent e é despachado pelo TIPO
// (nunca por nome em texto). PV-EVENT-011 (ADR-050): o Event Bus transporta só DOMAIN EVENTS;
// eventos técnicos de infraestrutura usam infraestrutura própria.
public abstract record EngineEvent
{
    public Guid EventId { get; init; } = Guid.NewGuid();
}

// PV-EVENT-011 (ADR-050): um combate contra um monstro terminou (hoje: monstro morto = vitória).
// Combat → Reward (→ futuro: Achievement/Quest) desacoplados por este evento.
public sealed record CombatCompletedEvent(
    Guid CombatResultId,
    Guid HuntId,
    Guid EncounterId,
    Guid CharacterId,
    Guid OpponentId,
    CombatOutcome Outcome) : EngineEvent
{
    public CombatResult ToCombatResult() =>
        new(CombatResultId, HuntId, EncounterId, CharacterId, OpponentId, Outcome);
}

// PV-EVENT-011 (spawn.md: "toda alteração relevante publica Engine Events"; fecha o R-046).
public sealed record SpawnInstanceActivatedEvent(
    Guid SpawnInstanceId,
    Guid SpawnDefinitionId,
    Guid HuntId,
    DateTime ActivatedAt) : EngineEvent;

public sealed record SpawnInstanceDespawnedEvent(
    Guid SpawnInstanceId,
    Guid SpawnDefinitionId,
    DateTime DespawnedAt) : EngineEvent;

// PV-EVENT-011 (revisão do PV-HUNT-012, ressalva 8): a Hunt terminou pelo pipeline único (ADR-012).
// O motivo vai como texto do enum da Application (o Domain não conhece HuntEndReason).
public sealed record HuntEndedEvent(
    Guid HuntId,
    Guid CharacterId,
    string Reason,
    long ElapsedSeconds,
    long ExperienceGained) : EngineEvent;
