using System;

namespace ProjectVictory.Application.Hunt;

public sealed class SelectHuntAreaHandler
{
    public SelectHuntAreaResult Handle(
        SelectHuntAreaCommand command)
    {
        ArgumentNullException.ThrowIfNull(command);

        var selected =
            command.CharacterId != Guid.Empty &&
            command.HuntAreaId != Guid.Empty;

        return new SelectHuntAreaResult(
            command.CharacterId,
            command.HuntAreaId,
            selected);
    }
}