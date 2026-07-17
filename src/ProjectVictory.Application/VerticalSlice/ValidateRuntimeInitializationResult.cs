using System;

namespace ProjectVictory.Application.VerticalSlice;

public sealed record ValidateRuntimeInitializationResult(
    Guid CharacterId,
    bool Initialized);