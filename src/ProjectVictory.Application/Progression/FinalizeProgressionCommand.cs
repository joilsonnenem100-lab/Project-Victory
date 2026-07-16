using System;

namespace ProjectVictory.Application.Progression;

public sealed record FinalizeProgressionCommand(
    Guid CharacterId,
    bool Confirmed);