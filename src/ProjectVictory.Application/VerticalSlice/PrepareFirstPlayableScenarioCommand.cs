using System;

namespace ProjectVictory.Application.VerticalSlice;

public sealed record PrepareFirstPlayableScenarioCommand(
    Guid CharacterId);