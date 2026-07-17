namespace ProjectVictory.Application.VerticalSlice;

public sealed class ValidateRuntimeRecoveryFlowHandler
{
    public ValidateRuntimeRecoveryFlowResult Handle(
        ValidateRuntimeRecoveryFlowCommand command)
    {
        ArgumentNullException.ThrowIfNull(command);

        if (command.CharacterId == Guid.Empty)
        {
            throw new InvalidOperationException(
                "CharacterId is required.");
        }

        return new ValidateRuntimeRecoveryFlowResult(
            command.CharacterId,
            true);
    }
}