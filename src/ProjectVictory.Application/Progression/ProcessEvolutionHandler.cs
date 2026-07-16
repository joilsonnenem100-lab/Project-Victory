using ProjectVictory.Domain.Progression;

namespace ProjectVictory.Application.Progression;

public sealed class ProcessEvolutionHandler
{
    private readonly ProgressionRule _progressionRule;

    public ProcessEvolutionHandler(
        ProgressionRule progressionRule)
    {
        _progressionRule = progressionRule;
    }

    public ProcessEvolutionResult Handle(
        ProcessEvolutionCommand command)
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

        return new ProcessEvolutionResult(
            decision.CharacterId,
            decision.CanEvolve);
    }
}