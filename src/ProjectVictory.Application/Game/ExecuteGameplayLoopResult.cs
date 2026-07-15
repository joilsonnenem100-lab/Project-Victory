namespace ProjectVictory.Application.Game;

public sealed record ExecuteGameplayLoopResult(
    Guid AccountId,
    Guid CharacterId,
    Guid HuntId,
    int Experience,
    int Level);