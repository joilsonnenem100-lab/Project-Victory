using System;
using ProjectVictory.Domain.Primitives;
using ProjectVictory.Domain.ValueObjects;

namespace ProjectVictory.Domain.Entities
{
    public sealed class Character : AggregateRoot
    {
        public Health Health { get; }

        public Mana Mana { get; }

        public Experience Experience { get; }

        public Level Level { get; }

        public Character(
            Guid id,
            Health health,
            Mana mana,
            Experience experience,
            Level level) : base(id)
        {
            Health = health;
            Mana = mana;
            Experience = experience;
            Level = level;
        }
    }
}