using System;

namespace ProjectVictory.Application.Experience;

public sealed record CreatePlayerStateViewResult(
    Guid CharacterId,
    bool Created);