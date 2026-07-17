using System;

namespace ProjectVictory.Application.VerticalSlice;

public sealed record ValidatePlayerExperienceCommand(
    Guid CharacterId);