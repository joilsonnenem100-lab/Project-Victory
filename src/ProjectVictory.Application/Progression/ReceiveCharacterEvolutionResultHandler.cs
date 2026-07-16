namespace ProjectVictory.Application.Progression;

public sealed class ReceiveCharacterEvolutionResultHandler
{
    public ReceiveCharacterEvolutionResultResult Handle(
        ReceiveCharacterEvolutionResultCommand command)
    {
        ArgumentNullException.ThrowIfNull(command);

        if (command.CharacterId == Guid.Empty)
        {
            throw new InvalidOperationException(
                "CharacterId is required.");
        }

        return new ReceiveCharacterEvolutionResultResult(
            command.CharacterId,
            command.Applied);
    }
}