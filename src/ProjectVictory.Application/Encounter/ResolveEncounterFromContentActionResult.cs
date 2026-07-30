using System;

namespace ProjectVictory.Application.Encounter;

public sealed record ResolveEncounterFromContentActionResult(
    Guid EncounterId,
    Guid LocationId,
    Guid ActionId,
    string Status);
