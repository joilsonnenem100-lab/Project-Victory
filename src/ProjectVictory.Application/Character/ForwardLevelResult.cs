using System;

namespace ProjectVictory.Application.Character;

public sealed record ForwardLevelResult(
    Guid CharacterId,
    bool Forwarded);