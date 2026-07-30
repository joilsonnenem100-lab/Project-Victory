using System;

namespace ProjectVictory.Domain.World;

public sealed record LocationContentDefinition(
    Guid Id,
    Guid LocationId,
    LocationContentType ContentType,
    LocationContentStatus Status);
