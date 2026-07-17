using System;

namespace ProjectVictory.Application.Inventory;

public sealed class ValidateInventoryHandler
{
    public ValidateInventoryResult Handle(
        ValidateInventoryCommand command)
    {
        ArgumentNullException.ThrowIfNull(command);

        if (command.CharacterId == Guid.Empty)
        {
            return new ValidateInventoryResult(
                command.CharacterId,
                false);
        }

        return new ValidateInventoryResult(
            command.CharacterId,
            true);
    }
}