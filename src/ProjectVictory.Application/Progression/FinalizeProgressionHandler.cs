using ProjectVictory.Domain.Progression;

namespace ProjectVictory.Application.Progression;

public sealed class FinalizeProgressionHandler
{
    public FinalizeProgressionResult Handle(
        FinalizeProgressionCommand command)
    {
        if (command.CharacterId == Guid.Empty)
        {
            throw new InvalidOperationException(
                "CharacterId is required.");
        }

        if (!command.Confirmed)
        {
            return new FinalizeProgressionResult(
                command.CharacterId,
                false);
        }

        var state = new ProgressionFinalState(
            command.CharacterId,
            true);

        return new FinalizeProgressionResult(
            state.CharacterId,
            state.Completed);
    }
}