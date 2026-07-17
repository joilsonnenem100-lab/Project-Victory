using System;

namespace ProjectVictory.Application.VerticalSlice;

public sealed record ValidateApplicationIntegrationResult(
    Guid CharacterId,
    bool Valid);