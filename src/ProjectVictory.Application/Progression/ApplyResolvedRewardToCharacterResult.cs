namespace ProjectVictory.Application.Progression;

public sealed record ApplyResolvedRewardToCharacterResult(
    Guid CharacterId,
    int ExperienceApplied,
    bool Applied);