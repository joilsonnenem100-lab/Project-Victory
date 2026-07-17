using System;

namespace ProjectVictory.Application.Progression;

public sealed record ValidateCharacterGrowthResult(
    Guid CharacterId,
    bool Valid);