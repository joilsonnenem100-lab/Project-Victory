using System;

namespace ProjectVictory.Domain.Progression;

public sealed record LevelResult(
    Guid CharacterId,
    int Level,
    bool Calculated);