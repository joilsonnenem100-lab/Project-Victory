using System;
using ProjectVictory.Domain.Primitives;

namespace ProjectVictory.Domain.Entities;

public sealed class Character : AggregateRoot
{
    public Character(Guid id)
        : base(id)
    {
    }
}