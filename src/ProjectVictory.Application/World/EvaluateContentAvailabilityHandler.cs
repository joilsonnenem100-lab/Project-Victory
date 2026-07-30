using System;

namespace ProjectVictory.Application.World;

public sealed class EvaluateContentAvailabilityHandler
{
    public EvaluateContentAvailabilityResult Handle(
        EvaluateContentAvailabilityCommand command)
    {
        if (command.ContentId == Guid.Empty)
        {
            throw new InvalidOperationException(
                "Content not found.");
        }

        return new EvaluateContentAvailabilityResult(
            command.ContentId,
            true);
    }
}
