using System;
using ProjectVictory.Domain.Account;
using ProjectVictory.Domain.Primitives;
using ProjectVictory.Domain.ValueObjects;

namespace ProjectVictory.Domain.Entities
{
    public sealed class Character : AggregateRoot
    {
        public AccountId AccountId { get; private set; }

        public Health Health { get; private set; }

        public Mana Mana { get; private set; }

        public Experience Experience { get; private set; }

        public Level Level { get; private set; }

        public Inventory Inventory { get; }

        public Equipment Equipment { get; }

        public Attributes Attributes { get; }

        public Skills Skills { get; }

        public Character(
            Guid id,
            AccountId accountId,
            Health health,
            Mana mana,
            Experience experience,
            Level level)
            : base(id)
        {
            ArgumentNullException.ThrowIfNull(accountId);
            ArgumentNullException.ThrowIfNull(health);
            ArgumentNullException.ThrowIfNull(mana);
            ArgumentNullException.ThrowIfNull(experience);
            ArgumentNullException.ThrowIfNull(level);

            AccountId = accountId;

            Health = health;
            Mana = mana;
            Experience = experience;
            Level = level;

            Inventory = new Inventory();
            Equipment = new Equipment();
            Attributes = new Attributes();
            Skills = new Skills();
        }

        public void Heal(Health health)
        {
            ArgumentNullException.ThrowIfNull(health);

            if (Health == health)
            {
                return;
            }

            Health = health;
        }

        public void SpendMana(Mana mana)
        {
            ArgumentNullException.ThrowIfNull(mana);

            if (Mana == mana)
            {
                return;
            }

            Mana = mana;
        }

        public void GainExperience(Experience experience)
        {
            ArgumentNullException.ThrowIfNull(experience);

            if (Experience == experience)
            {
                return;
            }

            Experience = experience;
        }
    }
}