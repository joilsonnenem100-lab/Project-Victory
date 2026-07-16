using System;

namespace ProjectVictory.Application.Character;

public sealed record ApplyLevelResult(
    Guid CharacterId,
    bool Applied);