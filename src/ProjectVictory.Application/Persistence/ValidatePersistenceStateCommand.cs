using System;

namespace ProjectVictory.Application.Persistence;

public sealed record ValidatePersistenceStateCommand(
    Guid CharacterId,
    Guid StateId,
    bool StateRepresentable);