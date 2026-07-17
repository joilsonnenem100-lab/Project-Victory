using System;

namespace ProjectVictory.Application.Hunt;

public sealed record StartEncounterResult(
    Guid CharacterId,
    Guid EncounterId,
    bool Started);