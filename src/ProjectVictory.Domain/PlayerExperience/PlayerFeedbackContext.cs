using System;

namespace ProjectVictory.Domain.PlayerExperience;

public sealed record PlayerFeedbackContext(
    Guid CharacterId,
    bool Success,
    string Message);