namespace ProjectVictory.Domain.Progression;

public sealed class ExperienceProcessor
{
    public ExperienceResult Process(
        ProgressionContext context,
        Experience experience)
    {
        ArgumentNullException.ThrowIfNull(context);
        ArgumentNullException.ThrowIfNull(experience);

        return new ExperienceResult(
            context.CharacterId,
            experience.Amount,
            true);
    }
}