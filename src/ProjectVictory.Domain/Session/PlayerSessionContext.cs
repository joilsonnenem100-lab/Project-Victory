using System;

namespace ProjectVictory.Domain.Session;

public sealed record PlayerSessionContext(
    Guid SessionId,
    Guid CharacterId,
    bool Active);