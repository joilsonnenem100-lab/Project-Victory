using ProjectVictory.Domain.Primitives;

namespace ProjectVictory.Domain.Combat.Events;

public sealed record DeathEvent(
    Guid TargetId) : IDomainEvent;
