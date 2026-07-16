using System;

namespace ProjectVictory.Domain.Progression;

public sealed class ProgressionContext
{
    public Guid Id { get; }

    public Guid CharacterId { get; }

    public int ExperienceAmount { get; }

    public ProgressionContext(
        Guid id,
        Guid characterId,
        int experienceAmount)
    {
        if (characterId == Guid.Empty)
        {
            throw new ArgumentException(
                "CharacterId is required.",
                nameof(characterId));
        }

        if (experienceAmount < 0)
        {
            throw new ArgumentException(
                "Experience amount cannot be negative.",
                nameof(experienceAmount));
        }

        Id = id;
        CharacterId = characterId;
        ExperienceAmount = experienceAmount;
    }
}