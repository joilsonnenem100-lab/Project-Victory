using System;

namespace ProjectVictory.Application.Persistence;

public sealed class ValidateRecoveryFlowHandler
{
    public ValidateRecoveryFlowResult Handle(
        ValidateRecoveryFlowCommand command)
    {
        ArgumentNullException.ThrowIfNull(command);

        var recovered =
            command.CharacterId != Guid.Empty &&
            command.StateId != Guid.Empty &&
            command.StateLoaded &&
            command.ContextRestored;

        return new ValidateRecoveryFlowResult(
            command.CharacterId,
            recovered);
    }
}