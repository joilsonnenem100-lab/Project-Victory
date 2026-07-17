using System;

namespace ProjectVictory.Application.Experience;

public sealed record CreatePlayerStateViewCommand(
    Guid CharacterId,
    Guid WorldId,
    Guid? HuntId,
    int Level);