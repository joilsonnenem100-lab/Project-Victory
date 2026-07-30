using System;

namespace ProjectVictory.Domain.World;

public sealed record LocationActionDefinition(
    Guid Id,
    Guid ContentId,
    LocationActionType ActionType,
    LocationActionStatus Status);
