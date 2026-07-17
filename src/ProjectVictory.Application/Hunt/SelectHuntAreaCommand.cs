using System;

namespace ProjectVictory.Application.Hunt;

public sealed record SelectHuntAreaCommand(
    Guid CharacterId,
    Guid HuntAreaId);