namespace ProjectVictory.Domain.Progression;

public sealed class GrowthProcessor
{
    public GrowthResult Process(
        ExperienceResult experienceResult,
        ExperienceTable table)
    {
        ArgumentNullException.ThrowIfNull(experienceResult);
        ArgumentNullException.ThrowIfNull(table);

        var level = 1;

        return new GrowthResult(
            experienceResult.CharacterId,
            experienceResult.ExperienceAmount,
            level,
            experienceResult.Processed);
    }
}