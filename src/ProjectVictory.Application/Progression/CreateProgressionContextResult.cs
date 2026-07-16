using System;

namespace ProjectVictory.Application.Progression;

public sealed record CreateProgressionContextResult(
    Guid ProgressionContextId,
    Guid CharacterId);