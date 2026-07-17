using System;

namespace ProjectVictory.Application.Experience;

public sealed record CreatePlayerExperienceContextCommand(
    Guid CharacterId,
    Guid WorldId,
    Guid? HuntId);