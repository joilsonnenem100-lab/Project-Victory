using ProjectVictory.Domain.Progression;

namespace ProjectVictory.Application.Progression;

public sealed class EvaluateProgressionRuleHandler
{
    private readonly ProgressionRule _rule;

    public EvaluateProgressionRuleHandler()
    {
        _rule = new ProgressionRule();
    }

    public EvaluateProgressionRuleResult Handle(
        EvaluateProgressionRuleCommand command)
    {
        ArgumentNullException.ThrowIfNull(command);

        var context =
            new ProgressionContext(
                Guid.NewGuid(),
                command.CharacterId,
                command.ExperienceAmount);

        var decision =
            _rule.Evaluate(context);

        return new EvaluateProgressionRuleResult(
            decision.CharacterId,
            decision.CanEvolve);
    }
}