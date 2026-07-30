using System;

namespace ProjectVictory.Application.Encounter;

public sealed record BindEncounterToRuntimeCommand(
    Guid RuntimeId,
    Guid EncounterId);
