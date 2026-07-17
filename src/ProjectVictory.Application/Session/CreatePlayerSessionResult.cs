using System;

namespace ProjectVictory.Application.Session;

public sealed record CreatePlayerSessionResult(
    Guid SessionId,
    Guid CharacterId,
    bool Created);