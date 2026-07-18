using System;

namespace ProjectVictory.Domain.Playtest;

public sealed record PlaytestFeedback(
    Guid Id,
    Guid CharacterId,
    string Description);