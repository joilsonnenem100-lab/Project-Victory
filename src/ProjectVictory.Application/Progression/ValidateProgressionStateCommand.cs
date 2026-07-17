using System;

namespace ProjectVictory.Application.Progression;

public sealed record ValidateProgressionStateCommand(
    Guid CharacterId);