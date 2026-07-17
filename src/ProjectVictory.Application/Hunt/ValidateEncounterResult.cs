using System;

namespace ProjectVictory.Application.Hunt;

public sealed record ValidateEncounterResult(
    Guid EncounterId,
    bool Valid);