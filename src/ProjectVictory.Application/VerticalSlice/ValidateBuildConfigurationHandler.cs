using System;

namespace ProjectVictory.Application.VerticalSlice;

public sealed class ValidateBuildConfigurationHandler
{
    public ValidateBuildConfigurationResult Handle(
        ValidateBuildConfigurationCommand command)
    {
        ArgumentNullException.ThrowIfNull(command);

        var valid =
            command.BuildId != Guid.Empty &&
            command.ConfigurationAvailable &&
            command.ProjectsResolved;

        return new ValidateBuildConfigurationResult(
            command.BuildId,
            valid);
    }
}