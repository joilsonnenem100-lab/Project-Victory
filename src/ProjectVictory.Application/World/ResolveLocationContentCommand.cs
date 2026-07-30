using System;

namespace ProjectVictory.Application.World;

public sealed record ResolveLocationContentCommand(
    Guid LocationId);
