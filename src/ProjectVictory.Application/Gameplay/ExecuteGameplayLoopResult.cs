using System;

namespace ProjectVictory.Application.Gameplay;

public sealed record ExecuteGameplayLoopResult(
    Guid CharacterId,
    Guid HuntId,
    bool Completed);