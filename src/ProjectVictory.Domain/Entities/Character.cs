using System;
using ProjectVictory.Domain.Primitives;
using ProjectVictory.Domain.ValueObjects;

namespace ProjectVictory.Domain.Entities
{
    public sealed class Character : AggregateRoot
    {
        public Health Health { get; private set; }

        public Mana Mana { get; private set; }

        public Experience Experience { get; private set; }

        public Level Level { get; }

        public Character(
            Guid id,
            Health health,
            Mana mana,
            Experience experience,
            Level level)
            : base(id)
        {
            Health = health;
            Mana = mana;
            Experience = experience;
            Level = level;
        }

        public void Heal(Health health)
        {
            Health = health;
        }

        public void SpendMana(Mana mana)
        {
            Mana = mana;
        }

        public void GainExperience(Experience experience)
        {
            Experience = experience;
        }
    }
}