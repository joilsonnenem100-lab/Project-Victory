using System;

namespace ProjectVictory.Application.VerticalSlice;

public sealed record PrepareInternalPlaytestCommand(
    Guid RuntimeId,
    Guid CharacterId,
    bool EnvironmentReady);