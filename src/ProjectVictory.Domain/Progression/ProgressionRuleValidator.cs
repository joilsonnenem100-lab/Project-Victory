namespace ProjectVictory.Domain.Progression;

public sealed class ProgressionRuleValidator
{
    public ProgressionDecisionValidationResult Validate(
        EvolutionDecision decision)
    {
        ArgumentNullException.ThrowIfNull(decision);

        return new ProgressionDecisionValidationResult(
            decision.CharacterId,
            true);
    }
}