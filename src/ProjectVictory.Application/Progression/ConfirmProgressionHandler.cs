using ProjectVictory.Domain.Progression;

namespace ProjectVictory.Application.Progression;

public sealed class ConfirmProgressionHandler
{
    public ConfirmProgressionResult Handle(
        ConfirmProgressionCommand command)
    {
        if (command.CharacterId == Guid.Empty)
        {
            throw new InvalidOperationException(
                "CharacterId is required.");
        }

        if (!command.CharacterApplicationCompleted)
        {
            return new ConfirmProgressionResult(
                command.CharacterId,
                false);
        }

        return new ConfirmProgressionResult(
            command.CharacterId,
            true);
    }
}