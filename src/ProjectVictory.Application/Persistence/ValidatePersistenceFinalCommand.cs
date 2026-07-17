using System;

namespace ProjectVictory.Application.Persistence;

public sealed record ValidatePersistenceFinalCommand(
    Guid CharacterId,
    bool StatePersisted,
    bool StateRecovered,
    bool RuntimeRestored);