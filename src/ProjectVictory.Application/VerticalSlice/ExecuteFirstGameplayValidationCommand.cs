using System;

namespace ProjectVictory.Application.VerticalSlice;

public sealed record ExecuteFirstGameplayValidationCommand(
    Guid CharacterId);