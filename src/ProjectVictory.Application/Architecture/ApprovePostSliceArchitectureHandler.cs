using System;

namespace ProjectVictory.Application.Architecture;

public sealed class ApprovePostSliceArchitectureHandler
{
    public ApprovePostSliceArchitectureResult Handle(
        ApprovePostSliceArchitectureCommand command)
    {
        ArgumentNullException.ThrowIfNull(command);

        var approved =
            command.ApprovalId != Guid.Empty &&
            command.ImpactApproved &&
            command.ConsistencyValidated;

        return new ApprovePostSliceArchitectureResult(
            command.ApprovalId,
            approved);
    }
}