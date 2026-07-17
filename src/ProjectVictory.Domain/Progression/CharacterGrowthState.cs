using System;

namespace ProjectVictory.Domain.Progression;

public sealed class CharacterGrowthState
{
    public Guid CharacterId { get; }

    public int ExperienceAmount { get; }

    public int CurrentLevel { get; }


    public CharacterGrowthState(
        Guid characterId,
        int experienceAmount,
        int currentLevel)
    {
        if (characterId == Guid.Empty)
        {
            throw new ArgumentException(
                "CharacterId is required.",
                nameof(characterId));
        }

        CharacterId = characterId;
        ExperienceAmount = experienceAmount;
        CurrentLevel = currentLevel;
    }
}