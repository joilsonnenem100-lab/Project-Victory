namespace ProjectVictory.Application.Character;

public sealed class ConfirmEvolutionSynchronizationHandler
{
    public ConfirmEvolutionSynchronizationResult Handle(
        ConfirmEvolutionSynchronizationCommand command)
    {
        if (command.CharacterId == Guid.Empty)
        {
            throw new InvalidOperationException(
                "CharacterId is required.");
        }

        return new ConfirmEvolutionSynchronizationResult(
            command.CharacterId,
            command.Compatible);
    }
}