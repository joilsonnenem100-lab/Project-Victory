namespace ProjectVictory.Application.Runtime;

public sealed record UpdateRuntimeActivityResult(
    Guid RuntimeId,
    string CurrentActivity);
