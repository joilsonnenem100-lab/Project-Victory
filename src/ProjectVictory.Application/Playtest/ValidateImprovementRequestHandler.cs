using System;

namespace ProjectVictory.Application.Playtest;

public sealed class ValidateImprovementRequestHandler
{
    public ValidateImprovementRequestResult Handle(
        ValidateImprovementRequestCommand command)
    {
        ArgumentNullException.ThrowIfNull(command);

        var validated =
            command.RequestId != Guid.Empty &&
            command.RequestDescribed &&
            command.ArchitecturalReviewRequired;

        return new ValidateImprovementRequestResult(
            command.RequestId,
            validated);
    }
}