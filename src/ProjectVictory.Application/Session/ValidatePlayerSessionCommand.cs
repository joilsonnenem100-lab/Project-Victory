using System;

namespace ProjectVictory.Application.Session;

public sealed record ValidatePlayerSessionCommand(
    Guid SessionId,
    Guid CharacterId,
    bool GameplayActive,
    bool PersistenceAvailable);