using ProjectVictory.Domain.Progression;

namespace ProjectVictory.Application.Progression;

public sealed class DefineProgressionRuleHandler
{
    private readonly ProgressionRule _progressionRule;

    public DefineProgressionRuleHandler()
    {
        _progressionRule = new ProgressionRule();
    }

    public DefineProgressionRuleResult Handle(
        DefineProgressionRuleCommand command)
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

        var decision = _progressionRule.Evaluate(
            context);

        return new DefineProgressionRuleResult(
            decision.CharacterId,
            decision.CanEvolve);
    }
}