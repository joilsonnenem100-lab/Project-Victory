namespace ProjectVictory.Application.Runtime;

public sealed record UpdateRuntimeActivityCommand(
    Guid RuntimeId,
    string Activity);
