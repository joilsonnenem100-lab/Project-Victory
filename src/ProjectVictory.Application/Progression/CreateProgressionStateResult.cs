using System;

namespace ProjectVictory.Application.Progression;

public sealed record CreateProgressionStateResult(
    Guid CharacterId,
    bool Created);