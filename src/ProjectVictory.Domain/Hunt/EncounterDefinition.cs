using System;

namespace ProjectVictory.Domain.Hunt;

public sealed record EncounterDefinition(
    Guid Id,
    Guid HuntAreaId,
    string Name);