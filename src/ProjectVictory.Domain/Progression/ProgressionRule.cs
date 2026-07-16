namespace ProjectVictory.Domain.Progression;

public sealed class ProgressionRule
{
    public EvolutionDecision Evaluate(
        ProgressionContext context)
    {
        ArgumentNullException.ThrowIfNull(context);

        if (context.ExperienceAmount < 0)
        {
            throw new InvalidOperationException(
                "Invalid progression effects.");
        }

        return new EvolutionDecision(
            context.CharacterId,
            context.ExperienceAmount > 0);
    }
}