using System;

namespace ProjectVictory.Application.Progression;

public sealed record CreateAdvancedProgressionResult(
    Guid CharacterId,
    bool Created);