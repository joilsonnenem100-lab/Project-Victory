using System;
using System.Collections.Generic;

namespace ProjectVictory.Application.World;

using ProjectVictory.Domain.World;

public sealed record ResolveLocationActionsResult(
    Guid LocationId,
    IReadOnlyList<LocationActionDefinition> AvailableActions);
