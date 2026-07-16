using System;

namespace ProjectVictory.Domain.Entities;

public sealed record LevelValidationResult(
    Guid CharacterId,
    bool Accepted);