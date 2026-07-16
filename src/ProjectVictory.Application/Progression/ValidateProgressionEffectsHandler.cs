using ProjectVictory.Domain.Progression;

namespace ProjectVictory.Application.Progression;

public sealed class ValidateProgressionEffectsHandler
{
    public ValidateProgressionEffectsResult Handle(
        ValidateProgressionEffectsCommand command)
    {
        if (command.CharacterId == Guid.Empty)
        {
            throw new InvalidOperationException(
                "CharacterId is required.");
        }

        if (command.ExperienceAmount < 0)
        {
            throw new InvalidOperationException(
                "Experience amount is invalid.");
        }

        var context = new ProgressionContext(
            Guid.NewGuid(),
            command.CharacterId,
            command.ExperienceAmount);

        ValidateContext(context);

        return new ValidateProgressionEffectsResult(
            context.CharacterId,
            true);
    }

    private static void ValidateContext(
        ProgressionContext context)
    {
        ArgumentNullException.ThrowIfNull(context);

        if (context.CharacterId == Guid.Empty)
        {
            throw new InvalidOperationException(
                "Progression context CharacterId is invalid.");
        }

        if (context.ExperienceAmount < 0)
        {
            throw new InvalidOperationException(
                "Progression context effects are invalid.");
        }
    }
}