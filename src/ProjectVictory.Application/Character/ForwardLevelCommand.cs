using System;

namespace ProjectVictory.Application.Character;

public sealed record ForwardLevelCommand(
    Guid CharacterId,
    int Level);