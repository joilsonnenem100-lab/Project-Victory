using System;

namespace ProjectVictory.Application.Hunt;

public sealed record ValidateHuntAreaResult(
    Guid HuntAreaId,
    bool Valid);