using ProjectVictory.Domain.Primitives;

namespace ProjectVictory.Domain.Combat.Events;

public sealed record DamageEvent(
    Guid SourceId,
    Guid TargetId,
    int Amount) : IDomainEvent;
