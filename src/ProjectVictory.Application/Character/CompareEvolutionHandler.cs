namespace ProjectVictory.Application.Character;

public sealed class CompareEvolutionHandler
{
    public CompareEvolutionResult Handle(
        CompareEvolutionCommand command)
    {
        if (command.CharacterId == Guid.Empty)
        {
            throw new InvalidOperationException(
                "CharacterId is required.");
        }

        var compatible =
            command.ProgressionLevel ==
            command.CharacterLevel;

        return new CompareEvolutionResult(
            command.CharacterId,
            compatible);
    }
}