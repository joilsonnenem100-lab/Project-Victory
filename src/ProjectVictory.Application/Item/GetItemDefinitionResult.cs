using System;

namespace ProjectVictory.Application.Item;

public sealed class GetItemDefinitionResult
{
    public Guid Id { get; init; }

    public string Name { get; init; } = string.Empty;

    public string Description { get; init; } = string.Empty;
}