using System;
using ProjectVictory.Domain.Progression;

namespace ProjectVictory.Application.Progression;

public sealed class CreateAdvancedProgressionHandler
{
    public CreateAdvancedProgressionResult Handle(
        CreateAdvancedProgressionCommand command)
    {
        ArgumentNullException.ThrowIfNull(command);

        var context =
            new AdvancedProgressionContext(
                command.CharacterId,
                command.ExperienceAmount,
                true);

        return new CreateAdvancedProgressionResult(
            context.CharacterId,
            true);
    }
}