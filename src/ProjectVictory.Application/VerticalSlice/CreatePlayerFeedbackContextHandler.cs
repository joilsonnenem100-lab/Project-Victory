using System;
using ProjectVictory.Domain.PlayerExperience;

namespace ProjectVictory.Application.VerticalSlice;

public sealed class CreatePlayerFeedbackContextHandler
{
    public CreatePlayerFeedbackContextResult Handle(
        CreatePlayerFeedbackContextCommand command)
    {
        ArgumentNullException.ThrowIfNull(command);

        var context = new PlayerFeedbackContext(
            command.CharacterId,
            command.Success,
            command.Message);

        return new CreatePlayerFeedbackContextResult(
            context.CharacterId,
            true);
    }
}