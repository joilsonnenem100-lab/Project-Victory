using ProjectVictory.Domain.Entities;

namespace ProjectVictory.Application.Character;

public sealed class CreateCharacterEvolutionResultHandler
{
    public CreateCharacterEvolutionResultResult Handle(
        CreateCharacterEvolutionResultCommand command)
    {
        ArgumentNullException.ThrowIfNull(command);

        if (command.CharacterId == Guid.Empty)
        {
            throw new InvalidOperationException(
                "CharacterId is required.");
        }

        var result = new CharacterEvolutionResult(
            command.CharacterId,
            command.Applied);

        return new CreateCharacterEvolutionResultResult(
            result.CharacterId,
            true);
    }
}