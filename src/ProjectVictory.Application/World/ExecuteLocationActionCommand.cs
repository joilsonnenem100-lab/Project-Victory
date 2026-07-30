using System;

namespace ProjectVictory.Application.World;

public sealed record ExecuteLocationActionCommand(
    Guid ActionId,
    Guid CharacterId);
