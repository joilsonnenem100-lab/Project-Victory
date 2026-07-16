namespace ProjectVictory.Application.Progression;

public sealed class CompleteEvolutionCycleHandler
{
    public CompleteEvolutionCycleResult Handle(
        CompleteEvolutionCycleCommand command)
    {
        ArgumentNullException.ThrowIfNull(command);

        if (command.CharacterId == Guid.Empty)
        {
            throw new InvalidOperationException(
                "CharacterId is required.");
        }

        return new CompleteEvolutionCycleResult(
            command.CharacterId,
            command.CharacterResultReceived);
    }
}