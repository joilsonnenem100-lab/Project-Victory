namespace ProjectVictory.Application.Runtime;

public sealed record RuntimeGameplaySynchronizationResult(
    Guid RuntimeId,
    string CurrentActivity,
    Guid? CurrentEncounterId);
