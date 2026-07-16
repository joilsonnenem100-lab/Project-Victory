using System;

namespace ProjectVictory.Application.Progression;

public sealed record ProcessProgressionResult(
    Guid CharacterId,
    Guid ProgressionContextId,
    bool Processed);