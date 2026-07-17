using System;

namespace ProjectVictory.Application.VerticalSlice;

public sealed record ValidateApplicationIntegrationCommand(
    Guid CharacterId);