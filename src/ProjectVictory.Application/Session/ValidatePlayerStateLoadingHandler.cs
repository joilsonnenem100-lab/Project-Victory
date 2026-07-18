using System;

namespace ProjectVictory.Application.Session;

public sealed class ValidatePlayerStateLoadingHandler
{
    public ValidatePlayerStateLoadingResult Handle(
        ValidatePlayerStateLoadingCommand command)
    {
        ArgumentNullException.ThrowIfNull(command);

        var valid =
            command.CharacterId != Guid.Empty &&
            command.CharacterLoaded;

        return new ValidatePlayerStateLoadingResult(
            command.CharacterId,
            valid);
    }
}