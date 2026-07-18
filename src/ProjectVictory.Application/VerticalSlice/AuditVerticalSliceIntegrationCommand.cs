using System;

namespace ProjectVictory.Application.VerticalSlice;

public sealed record AuditVerticalSliceIntegrationCommand(
    Guid CharacterId,
    bool AccountValid,
    bool GameplayValid,
    bool PersistenceValid);