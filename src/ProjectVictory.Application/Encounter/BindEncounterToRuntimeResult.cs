using System;

namespace ProjectVictory.Application.Encounter;

public sealed record BindEncounterToRuntimeResult(
    Guid RuntimeId,
    Guid EncounterId);
