using System;

namespace ProjectVictory.Domain.Gameplay;

public sealed record GameplaySessionContext(
    Guid SessionId,
    Guid CharacterId);