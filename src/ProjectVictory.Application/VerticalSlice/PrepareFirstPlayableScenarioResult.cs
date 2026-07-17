using System;

namespace ProjectVictory.Application.VerticalSlice;

public sealed record PrepareFirstPlayableScenarioResult(
    Guid CharacterId,
    bool Prepared);