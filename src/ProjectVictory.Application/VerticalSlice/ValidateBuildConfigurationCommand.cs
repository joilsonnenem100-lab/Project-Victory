using System;

namespace ProjectVictory.Application.VerticalSlice;

public sealed record ValidateBuildConfigurationCommand(
    Guid BuildId,
    bool ConfigurationAvailable,
    bool ProjectsResolved);