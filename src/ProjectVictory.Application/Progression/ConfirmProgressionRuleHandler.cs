namespace ProjectVictory.Application.Progression;

public sealed class ConfirmProgressionRuleHandler
{
    public ConfirmProgressionRuleResult Handle(
        ConfirmProgressionRuleCommand command)
    {
        if (command.CharacterId == Guid.Empty)
        {
            throw new InvalidOperationException(
                "CharacterId is required.");
        }

        return new ConfirmProgressionRuleResult(
            command.CharacterId,
            command.Validated);
    }
}