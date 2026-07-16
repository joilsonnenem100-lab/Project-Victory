namespace ProjectVictory.Domain.Progression;

public sealed class LevelCalculator
{
    public Level Calculate(
        Experience experience)
    {
        ArgumentNullException.ThrowIfNull(experience);

        var calculatedLevel =
            (experience.Amount / 100) + 1;

        return new Level(
            calculatedLevel);
    }
}