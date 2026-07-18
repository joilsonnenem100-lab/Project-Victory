using System;

namespace ProjectVictory.Application.Architecture;

public sealed record ValidateArchitectureConsistencyCommand(
    Guid ValidationId,
    bool BoundariesPreserved,
    bool ContractsPreserved);