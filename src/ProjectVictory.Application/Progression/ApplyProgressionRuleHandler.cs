using ProjectVictory.Domain.Progression;

namespace ProjectVictory.Application.Progression;

public sealed class ApplyProgressionRuleHandler
{
    private readonly ProgressionRule _rule;
    private readonly ProgressionRuleValidator _validator;

    public ApplyProgressionRuleHandler()
    {
        _rule = new ProgressionRule();
        _validator = new ProgressionRuleValidator();
    }

    public ApplyProgressionRuleResult Handle(
        ApplyProgressionRuleCommand command)
    {
        if (command.CharacterId == Guid.Empty)
        {
            throw new InvalidOperationException(
                "CharacterId is required.");
        }

        var context = new ProgressionContext(
            Guid.NewGuid(),
            command.CharacterId,
            command.ExperienceAmount);

        var decision = _rule.Evaluate(context);

        var validation = _validator.Validate(
            decision);

        return new ApplyProgressionRuleResult(
            validation.CharacterId,
            validation.Valid);
    }
}