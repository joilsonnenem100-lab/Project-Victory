using System;

namespace ProjectVictory.Application.Persistence;

public sealed record ValidateRecoveryFlowResult(
    Guid CharacterId,
    bool Recovered);