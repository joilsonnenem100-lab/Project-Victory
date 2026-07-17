using System;

namespace ProjectVictory.Application.Hunt;

public sealed record IntegrateGameplayContentResult(
    Guid HuntAreaId,
    bool Integrated);