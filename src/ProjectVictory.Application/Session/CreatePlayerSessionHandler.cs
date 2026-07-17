using System;
using ProjectVictory.Domain.Session;

namespace ProjectVictory.Application.Session;

public sealed class CreatePlayerSessionHandler
{
    public CreatePlayerSessionResult Handle(
        CreatePlayerSessionCommand command)
    {
        ArgumentNullException.ThrowIfNull(command);

        var context =
            new PlayerSessionContext(
                command.SessionId,
                command.CharacterId,
                true);

        return new CreatePlayerSessionResult(
            context.SessionId,
            context.CharacterId,
            true);
    }
}