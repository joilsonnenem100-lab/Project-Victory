using System;

namespace ProjectVictory.Application.Runtime;

public sealed record ChangeRuntimeLocationResult(
    Guid RuntimeId,
    Guid LocationId);
