using System;

namespace ProjectVictory.Application.VerticalSlice;

public sealed class ValidatePlayerFeedbackHandler
{
    public ValidatePlayerFeedbackResult Handle(
        ValidatePlayerFeedbackCommand command)
    {
        ArgumentNullException.ThrowIfNull(command);

        var valid =
            command.CharacterId != Guid.Empty &&
            command.GameplayCompleted &&
            command.FeedbackGenerated;

        return new ValidatePlayerFeedbackResult(
            command.CharacterId,
            valid);
    }
}