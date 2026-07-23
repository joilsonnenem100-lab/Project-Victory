namespace ProjectVictory.Domain.Progression;

public sealed class GrowthProcessor
{
    public GrowthResult Process(
        ExperienceResult experienceResult,
        ExperienceTable table)
    {
        ArgumentNullException.ThrowIfNull(experienceResult);
        ArgumentNullException.ThrowIfNull(table);


        var levelCalculator =
            new LevelCalculator();


        var level =
            levelCalculator.Calculate(
                new Experience(
                    experienceResult.ExperienceAmount));


        return new GrowthResult(
            experienceResult.CharacterId,
            experienceResult.ExperienceAmount,
            level.Value,
            experienceResult.Processed);
    }
}