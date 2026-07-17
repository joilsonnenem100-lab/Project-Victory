using System;

namespace ProjectVictory.Application.Session;

public sealed record CreatePlayerSessionCommand(
    Guid SessionId,
    Guid CharacterId);