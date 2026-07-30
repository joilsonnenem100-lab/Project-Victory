using System;

namespace ProjectVictory.Application.Session;

public sealed class ValidateVerticalSliceSessionFlowHandler
{
    public ValidateVerticalSliceSessionFlowResult Handle(
        ValidateVerticalSliceSessionFlowCommand command)
    {
        ArgumentNullException.ThrowIfNull(command);

        var valid =
            command.CharacterId != Guid.Empty &&
            command.RuntimeId != Guid.Empty;

        return new ValidateVerticalSliceSessionFlowResult(
            command.CharacterId,
            command.RuntimeId,
            valid);
    }
}
