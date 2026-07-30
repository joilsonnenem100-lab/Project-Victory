using System;

namespace ProjectVictory.Application.Encounter;

public sealed record ResolveEncounterFromContentActionCommand(
    Guid LocationId,
    Guid ActionId);
