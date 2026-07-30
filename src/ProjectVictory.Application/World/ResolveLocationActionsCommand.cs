using System;

namespace ProjectVictory.Application.World;

public sealed record ResolveLocationActionsCommand(
    Guid LocationId);
