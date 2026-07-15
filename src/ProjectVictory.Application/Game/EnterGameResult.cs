namespace ProjectVictory.Application.Game;

public sealed record EnterGameResult(
    Guid AccountId,
    Guid CharacterId);