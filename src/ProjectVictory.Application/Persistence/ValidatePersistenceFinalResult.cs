using System;

namespace ProjectVictory.Application.Persistence;

public sealed record ValidatePersistenceFinalResult(
    Guid CharacterId,
    bool Completed);