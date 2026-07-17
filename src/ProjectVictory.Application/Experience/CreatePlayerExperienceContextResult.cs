using System;

namespace ProjectVictory.Application.Experience;

public sealed record CreatePlayerExperienceContextResult(
    Guid CharacterId,
    bool Created);