using System;

namespace ProjectVictory.Application.Progression;

public sealed record ValidateProgressionRecoveryCommand(
    Guid CharacterId);