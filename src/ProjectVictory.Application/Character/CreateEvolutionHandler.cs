namespace ProjectVictory.Application.Character;

public sealed class CreateEvolutionHandler
{
    public CreateEvolutionResult Handle(
        CreateEvolutionCommand command)
    {
        if (command.CharacterId == Guid.Empty)
        {
            throw new InvalidOperationException(
                "CharacterId is required.");
        }

        if (command.EvolutionResultId == Guid.Empty)
        {
            throw new InvalidOperationException(
                "EvolutionResultId is required.");
        }

        return new CreateEvolutionResult(
            command.CharacterId,
            command.EvolutionAvailable);
    }
}