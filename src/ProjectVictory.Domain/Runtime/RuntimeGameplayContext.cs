namespace ProjectVictory.Domain.Runtime;

public sealed record RuntimeGameplayContext(
    RuntimeActivity CurrentActivity,
    Guid? CurrentHuntId = null,
    Guid? CurrentEncounterId = null,
    RuntimeTemporaryGameplayState? TemporaryState = null);
