using System;

namespace ProjectVictory.Application.VerticalSlice;

public sealed record ValidateRuntimeInitializationCommand(
    Guid CharacterId);