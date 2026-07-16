using System;

namespace ProjectVictory.Application.Character;

public sealed record ValidateLevelResult(
    Guid CharacterId,
    bool Accepted);