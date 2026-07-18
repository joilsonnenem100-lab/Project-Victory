using System;

namespace ProjectVictory.Application.VerticalSlice;

public sealed record InitializePlayableEnvironmentResult(
    Guid RuntimeId,
    bool Initialized);