using System;

namespace ProjectVictory.Application.World;

public sealed record ExplorationDecisionCommand(
    Guid RuntimeId,
    Guid ActionId,
    Guid CharacterId);
