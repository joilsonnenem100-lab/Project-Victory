using System;

namespace ProjectVictory.Application.VerticalSlice;

public sealed record ValidatePersistenceIntegrationResult(
    Guid CharacterId,
    bool Valid);