using System;
using System.Collections.Generic;

namespace ProjectVictory.Application.World;

public sealed record ResolveLocationContentResult(
    Guid LocationId,
    IReadOnlyList<string> AvailableContents);
