namespace ProjectVictory.Application.Progression;

public sealed record ApplyResolvedRewardToCharacterResult(
    Guid CharacterId,
    long ExperienceApplied,
    bool Applied);