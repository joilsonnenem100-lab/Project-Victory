using System;
using ProjectVictory.Domain.Progression;

namespace ProjectVictory.Application.Progression;

public sealed class CreateCharacterGrowthHandler
{
    public CreateCharacterGrowthResult Handle(
        CreateCharacterGrowthCommand command)
    {
        ArgumentNullException.ThrowIfNull(command);

        var state =
            new CharacterGrowthState(
                command.CharacterId,
                command.ExperienceAmount,
                command.Level);

        return new CreateCharacterGrowthResult(
            state.CharacterId,
            true);
    }
}