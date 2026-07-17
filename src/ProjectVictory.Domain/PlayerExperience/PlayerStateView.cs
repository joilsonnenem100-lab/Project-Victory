using System;

namespace ProjectVictory.Domain.PlayerExperience;

public sealed record PlayerStateView(
    Guid CharacterId,
    Guid WorldId,
    Guid? HuntId,
    int Level);