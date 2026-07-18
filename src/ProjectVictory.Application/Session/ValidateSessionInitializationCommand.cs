using System;

namespace ProjectVictory.Application.Session;

public sealed record ValidateSessionInitializationCommand(
    Guid SessionId,
    Guid CharacterId);