using System;

namespace ProjectVictory.Application.Hunt;

public sealed record StartEncounterCommand(
    Guid CharacterId,
    Guid HuntAreaId,
    Guid EncounterId);