using System;

namespace ProjectVictory.Application.VerticalSlice;

public sealed record InitializePlayableEnvironmentCommand(
    Guid RuntimeId,
    Guid CharacterId);