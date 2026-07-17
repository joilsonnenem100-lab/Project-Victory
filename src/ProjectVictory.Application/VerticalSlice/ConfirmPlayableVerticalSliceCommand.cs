using System;

namespace ProjectVictory.Application.VerticalSlice;

public sealed record ConfirmPlayableVerticalSliceCommand(
    Guid CharacterId);