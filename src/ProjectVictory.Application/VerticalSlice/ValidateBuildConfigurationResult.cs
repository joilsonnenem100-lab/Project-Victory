using System;

namespace ProjectVictory.Application.VerticalSlice;

public sealed record ValidateBuildConfigurationResult(
    Guid BuildId,
    bool Valid);