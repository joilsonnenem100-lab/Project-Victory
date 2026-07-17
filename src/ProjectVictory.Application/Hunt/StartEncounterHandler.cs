using System;

namespace ProjectVictory.Application.Hunt;

public sealed class StartEncounterHandler
{
    public StartEncounterResult Handle(
        StartEncounterCommand command)
    {
        ArgumentNullException.ThrowIfNull(command);

        var started =
            command.CharacterId != Guid.Empty &&
            command.HuntAreaId != Guid.Empty &&
            command.EncounterId != Guid.Empty;

        return new StartEncounterResult(
            command.CharacterId,
            command.EncounterId,
            started);
    }
}