using System;

namespace ProjectVictory.Application.World;

public sealed record ExecuteLocationActionResult(
    Guid ActionId,
    string System);
