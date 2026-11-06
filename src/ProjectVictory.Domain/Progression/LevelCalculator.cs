namespace ProjectVictory.Domain.Progression;

public sealed class LevelCalculator
{
    public Level Calculate(
        Experience experience)
    {
        ArgumentNullException.ThrowIfNull(experience);

        // PV-PROG-011: o Level vem da Experience Table (ADR-025), não de fórmula própria.
        var calculatedLevel =
            ExperienceTable.ForAscension(ExperienceTable.FirstAscension).GetLevel(
                experience.Amount);

        return new Level(
            calculatedLevel);
    }
}