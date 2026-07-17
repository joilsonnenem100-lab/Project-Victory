using System;

namespace ProjectVictory.Application.Equipment;

public sealed class ValidateEquipmentHandler
{
    public ValidateEquipmentResult Handle(
        ValidateEquipmentCommand command)
    {
        ArgumentNullException.ThrowIfNull(command);

        return new ValidateEquipmentResult(
            command.CharacterId,
            command.CharacterId != Guid.Empty);
    }
}