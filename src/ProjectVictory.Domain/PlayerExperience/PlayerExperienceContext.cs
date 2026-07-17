using System;

namespace ProjectVictory.Domain.PlayerExperience;

public sealed record PlayerExperienceContext(
    Guid CharacterId,
    Guid WorldId,
    Guid? HuntId);