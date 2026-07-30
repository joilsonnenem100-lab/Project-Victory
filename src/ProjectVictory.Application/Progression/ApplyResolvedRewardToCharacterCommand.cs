namespace ProjectVictory.Application.Progression;

public sealed record ApplyResolvedRewardToCharacterCommand(
    Guid CharacterId,
    int? ExperienceAmount);