using System;

namespace ProjectVictory.Application.Gameplay;

public sealed record SelectHuntResult(
    Guid HuntId,
    bool Selected);