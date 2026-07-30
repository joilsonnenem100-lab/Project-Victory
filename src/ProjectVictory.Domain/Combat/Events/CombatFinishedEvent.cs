using ProjectVictory.Domain.Primitives;

namespace ProjectVictory.Domain.Combat.Events;

public sealed record CombatFinishedEvent(
    Guid EncounterId,
    string Result) : IDomainEvent;
