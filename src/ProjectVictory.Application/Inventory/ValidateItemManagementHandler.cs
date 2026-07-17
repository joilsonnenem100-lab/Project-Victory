using System;

namespace ProjectVictory.Application.Inventory;

public sealed class ValidateItemManagementHandler
{
    public ValidateItemManagementResult Handle(
        ValidateItemManagementCommand command)
    {
        ArgumentNullException.ThrowIfNull(command);

        return new ValidateItemManagementResult(
            command.CharacterId,
            command.CharacterId != Guid.Empty);
    }
}