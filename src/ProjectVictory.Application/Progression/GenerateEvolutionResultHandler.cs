using ProjectVictory.Domain.Progression;

namespace ProjectVictory.Application.Progression;

public sealed class GenerateEvolutionResultHandler
{
    private readonly ProgressionRule _progressionRule;

    public GenerateEvolutionResultHandler(
        ProgressionRule progressionRule)
    {
        _progressionRule = progressionRule;
    }

    public EvolutionResult Handle(
        GenerateEvolutionResultCommand command)
    {
        if (command.CharacterId == Guid.Empty)
        {
            throw new InvalidOperationException(
                "CharacterId is required.");
        }

        if (command.ExperienceAmount < 0)
        {
            throw new InvalidOperationException(
                "Experience amount is invalid.");
        }

        var context = new ProgressionContext(
            Guid.NewGuid(),
            command.CharacterId,
            command.ExperienceAmount);

        EvolutionDecision decision =
            _progressionRule.Evaluate(context);

        return new EvolutionResult(
            decision.CharacterId,
            decision.CanEvolve);
    }
}