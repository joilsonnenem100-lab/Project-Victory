using System;

namespace ProjectVictory.Application.VerticalSlice;

public sealed record AuditVerticalSliceIntegrationResult(
    Guid CharacterId,
    bool Valid);