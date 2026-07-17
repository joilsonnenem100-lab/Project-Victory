using System;

namespace ProjectVictory.Application.VerticalSlice;

public sealed record ValidatePersistenceIntegrationCommand(
    Guid CharacterId);