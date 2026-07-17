using System;

namespace ProjectVictory.Domain.PlayerExperience;

public sealed record PlayerExperienceState(
    Guid CharacterId,
    Guid SessionId,
    bool GameplayActive);