using System;

namespace ProjectVictory.Application.Progression;

public sealed record ConfirmProgressionCommand(
    Guid CharacterId,
    bool CharacterApplicationCompleted);