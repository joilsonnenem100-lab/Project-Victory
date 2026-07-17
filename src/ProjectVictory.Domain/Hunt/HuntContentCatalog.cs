using System;
using System.Collections.Generic;

namespace ProjectVictory.Domain.Hunt;

public sealed class HuntContentCatalog
{
    private readonly Dictionary<Guid, HuntContentReference> _contents = new();

    public void Add(
        HuntContentReference content)
    {
        ArgumentNullException.ThrowIfNull(content);

        _contents[content.ContentId] = content;
    }

    public HuntContentReference? Get(
        Guid contentId)
    {
        _contents.TryGetValue(
            contentId,
            out var content);

        return content;
    }
}