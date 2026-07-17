using System;

namespace ProjectVictory.Application.Gameplay;

public sealed record ExecuteGameplayLoopCommand(
    Guid CharacterId,
    Guid HuntId);