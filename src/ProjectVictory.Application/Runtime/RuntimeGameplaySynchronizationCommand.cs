namespace ProjectVictory.Application.Runtime;

public sealed record RuntimeGameplaySynchronizationCommand(
    Guid RuntimeId,
    string Activity,
    Guid? EncounterId = null);
