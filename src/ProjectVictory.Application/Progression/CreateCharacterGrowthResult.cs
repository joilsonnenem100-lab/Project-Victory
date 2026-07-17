using System;

namespace ProjectVictory.Application.Progression;

public sealed record CreateCharacterGrowthResult(
    Guid CharacterId,
    bool Created);