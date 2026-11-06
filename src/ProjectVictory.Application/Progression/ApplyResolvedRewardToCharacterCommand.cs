namespace ProjectVictory.Application.Progression;

public sealed record ApplyResolvedRewardToCharacterCommand(
    Guid CharacterId,
    long? ExperienceAmount);