using System;

namespace ProjectVictory.Application.World;

public sealed record ExplorationDecisionResult(
    Guid RuntimeId,
    Guid ActionId,
    Guid CharacterId,
    string Result);
