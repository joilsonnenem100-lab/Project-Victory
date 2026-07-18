using System;

namespace ProjectVictory.Application.VerticalSlice;

public sealed class AuditVerticalSliceIntegrationHandler
{
    public AuditVerticalSliceIntegrationResult Handle(
        AuditVerticalSliceIntegrationCommand command)
    {
        ArgumentNullException.ThrowIfNull(command);

        var valid =
            command.CharacterId != Guid.Empty &&
            command.AccountValid &&
            command.GameplayValid &&
            command.PersistenceValid;

        return new AuditVerticalSliceIntegrationResult(
            command.CharacterId,
            valid);
    }
}