using System;

namespace ProjectVictory.Application.VerticalSlice;

public sealed record ApproveVerticalSliceFinalCommand(
    Guid CharacterId,
    bool IntegrationApproved,
    bool PlaytestPrepared,
    bool RuntimeValidated);