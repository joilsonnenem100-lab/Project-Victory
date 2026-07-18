using System;

namespace ProjectVictory.Application.VerticalSlice;

public sealed class ApproveVerticalSliceFinalHandler
{
    public ApproveVerticalSliceFinalResult Handle(
        ApproveVerticalSliceFinalCommand command)
    {
        ArgumentNullException.ThrowIfNull(command);

        var approved =
            command.CharacterId != Guid.Empty &&
            command.IntegrationApproved &&
            command.PlaytestPrepared &&
            command.RuntimeValidated;

        return new ApproveVerticalSliceFinalResult(
            command.CharacterId,
            approved);
    }
}