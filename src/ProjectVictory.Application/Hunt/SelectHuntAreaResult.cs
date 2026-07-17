using System;

namespace ProjectVictory.Application.Hunt;

public sealed record SelectHuntAreaResult(
    Guid CharacterId,
    Guid HuntAreaId,
    bool Selected);