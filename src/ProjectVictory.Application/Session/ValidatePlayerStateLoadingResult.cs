using System;

namespace ProjectVictory.Application.Session;

public sealed record ValidatePlayerStateLoadingResult(
    Guid CharacterId,
    bool Valid);