using System;

namespace ProjectVictory.Application.Persistence;

public sealed record ValidateRecoveryFlowCommand(
    Guid CharacterId,
    Guid StateId,
    bool StateLoaded,
    bool ContextRestored);